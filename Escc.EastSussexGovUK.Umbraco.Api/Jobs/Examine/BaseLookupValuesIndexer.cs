using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Examine;
using Examine.LuceneEngine;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Security;
using Exceptionless;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine
{
    /// <summary>
    /// Examine indexer for indexing lookup values for searching jobs
    /// </summary>
    public abstract class BaseLookupValuesIndexer : ISimpleDataService
    {
        private readonly IJobsLookupValuesProvider _lookupValuesProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJobsIndexer" /> class.
        /// </summary>
        /// <param name="lookupValuesProvider">The lookup values provider.</param>
        /// <exception cref="System.ArgumentNullException">stopWordsRemover</exception>
        protected BaseLookupValuesIndexer(IJobsLookupValuesProvider lookupValuesProvider)
        {
            if (lookupValuesProvider == null) throw new ArgumentNullException(nameof(lookupValuesProvider));
            _lookupValuesProvider = lookupValuesProvider;
        }

        /// <summary>
        /// Gets the jobs data provider used to check how many jobs match each lookup value
        /// </summary>
        /// <returns></returns>
        /// <remarks>This cannot be injected into the constructor as, when the provider uses Examine, it is too early in the lifecycle and crashes IIS!</remarks>
        protected abstract IJobsDataProvider GetJobsDataProvider();

        public IEnumerable<SimpleDataSet> GetAllData(string indexType)
        {
            // Because this calls Task<T>.Result to get the result of an async method, every await call from here down
            // has to be suffixed with .ConfigureAwait(false) to avoid deadlocks
            // https://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async
            return GetAllDataAsync(indexType).Result;
        }

        public async Task<IEnumerable<SimpleDataSet>> GetAllDataAsync(string indexType)
        {
            //Ensure that an Umbraco context is available
            if (UmbracoContext.Current == null)
            {
                var dummyContext =
                    new HttpContextWrapper(
                        new HttpContext(new SimpleWorkerRequest("/", string.Empty, new StringWriter())));
                UmbracoContext.EnsureContext(
                    dummyContext,
                    ApplicationContext.Current,
                    new WebSecurity(dummyContext, ApplicationContext.Current),
                    false);
            }

            var dataSets = new List<SimpleDataSet>();

            try

            {
                var jobsDataProvider = GetJobsDataProvider();

                var i = 1;

                var locations = await _lookupValuesProvider.ReadLocations().ConfigureAwait(false);
                foreach (var lookupValue in locations)
                {
                    var query = new JobSearchQuery();
                    query.Locations.Add(lookupValue.Text);
                    query.ClosingDateFrom = DateTime.Today;
                    var simpleDataSet = await CreateDataSetFromLookup(i, indexType, "Location", query, lookupValue, jobsDataProvider).ConfigureAwait(false);
                    dataSets.Add(simpleDataSet);
                    i++;
                }

                var jobTypes = await _lookupValuesProvider.ReadJobTypes().ConfigureAwait(false);
                foreach (var lookupValue in jobTypes)
                {
                    var query = new JobSearchQuery();
                    query.JobTypes.Add(lookupValue.Text);
                    query.ClosingDateFrom = DateTime.Today;
                    var simpleDataSet = await CreateDataSetFromLookup(i, indexType, "JobType", query, lookupValue, jobsDataProvider).ConfigureAwait(false);
                    dataSets.Add(simpleDataSet);
                    i++;
                }

                var salaryRanges = await _lookupValuesProvider.ReadSalaryRanges().ConfigureAwait(false);
                foreach (var lookupValue in salaryRanges)
                {
                    lookupValue.Text = lookupValue.Text.Replace(" - ", " to "); // East Sussex County Council house style

                    var query = new JobSearchQuery();
                    query.SalaryRanges.Add(lookupValue.Text.Replace(",",String.Empty));
                    query.ClosingDateFrom = DateTime.Today;
                    var simpleDataSet = await CreateDataSetFromLookup(i, indexType, "SalaryRange", query, lookupValue, jobsDataProvider).ConfigureAwait(false);
                    dataSets.Add(simpleDataSet);
                    i++;
                }

                var salaryFrequencies = await _lookupValuesProvider.ReadSalaryFrequencies().ConfigureAwait(false);
                foreach (var lookupValue in salaryFrequencies)
                {
                    var simpleDataSet = await CreateDataSetFromLookup(i, indexType, "SalaryFrequency", null, lookupValue, null).ConfigureAwait(false);
                    dataSets.Add(simpleDataSet);
                    i++;
                }

                var organisations = await _lookupValuesProvider.ReadOrganisations().ConfigureAwait(false);
                foreach (var lookupValue in organisations)
                {
                    var query = new JobSearchQuery();
                    query.Organisations.Add(lookupValue.Text);
                    query.ClosingDateFrom = DateTime.Today;
                    var simpleDataSet = await CreateDataSetFromLookup(i, indexType, "Organisation", query, lookupValue, jobsDataProvider).ConfigureAwait(false);
                    dataSets.Add(simpleDataSet);
                    i++;
                }

                var workPatterns = await _lookupValuesProvider.ReadWorkPatterns().ConfigureAwait(false);
                foreach (var lookupValue in workPatterns)
                {
                    var query = new JobSearchQuery();
                    query.WorkPatterns.Add(lookupValue.Text);
                    query.ClosingDateFrom = DateTime.Today;
                    var simpleDataSet = await CreateDataSetFromLookup(i, indexType, "WorkPattern", query, lookupValue, jobsDataProvider).ConfigureAwait(false);
                    dataSets.Add(simpleDataSet);
                    i++;
                }

                var contractTypes = await _lookupValuesProvider.ReadContractTypes().ConfigureAwait(false);
                foreach (var lookupValue in contractTypes)
                {
                    var simpleDataSet = await CreateDataSetFromLookup(i, indexType, "ContractType", null, lookupValue, null).ConfigureAwait(false);
                    dataSets.Add(simpleDataSet);
                    i++;
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<BaseJobsIndexer>("error indexing:", ex);
            }

            LogHelper.Info<BaseLookupValuesIndexer>($"{dataSets.Count} items built for indexing by {this.GetType().ToString()}");

            return dataSets;
        }

        private static async Task<SimpleDataSet> CreateDataSetFromLookup(int fakeNodeId, string indexType, string group, JobSearchQuery query, JobsLookupValue lookupValue, IJobsDataProvider jobsDataProvider)
        {
            var simpleDataSet = new SimpleDataSet {NodeDefinition = new IndexedNode(), RowData = new Dictionary<string, string>()};

            simpleDataSet.NodeDefinition.NodeId = fakeNodeId;
            simpleDataSet.NodeDefinition.Type = indexType;
            simpleDataSet.RowData.Add("id", lookupValue.LookupValueId);
            simpleDataSet.RowData.Add("group", group);
            simpleDataSet.RowData.Add("text", lookupValue.Text);

            if (jobsDataProvider != null)
            {
                var jobs = await jobsDataProvider.ReadJobs(query).ConfigureAwait(false);
                simpleDataSet.RowData.Add("count", jobs.TotalJobs.ToString(CultureInfo.CurrentCulture));
            }
            return simpleDataSet;
        }
    }
}