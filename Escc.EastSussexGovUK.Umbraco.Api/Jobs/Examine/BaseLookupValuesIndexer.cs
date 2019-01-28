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
            if (UmbracoContext.Current == null && ApplicationContext.Current != null)
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
                dataSets.AddRange(await CreateDataSetFromLookupValues(indexType, "Location", i, locations, (lookup, query) => query.Locations.Add(lookup.Text), jobsDataProvider).ConfigureAwait(false));
                if (locations != null) i = i + locations.Count;

                var jobTypes = await _lookupValuesProvider.ReadJobTypes().ConfigureAwait(false);
                dataSets.AddRange(await CreateDataSetFromLookupValues(indexType, "JobType", i, jobTypes, (lookup, query) => query.JobTypes.Add(lookup.Text), jobsDataProvider).ConfigureAwait(false));
                if (jobTypes != null) i = i + jobTypes.Count;

                var salaryRanges = await _lookupValuesProvider.ReadSalaryRanges().ConfigureAwait(false);
                if (salaryRanges != null)
                {
                    foreach (var lookupValue in salaryRanges)
                    {
                        lookupValue.Text = lookupValue.Text.Replace(" - ", " to "); // East Sussex County Council house style
                    }
                }
                dataSets.AddRange(await CreateDataSetFromLookupValues(indexType, "SalaryRange", i, salaryRanges, (lookup, query) => query.SalaryRanges.Add(lookup.Text.Replace(",", String.Empty)), jobsDataProvider).ConfigureAwait(false));
                if (salaryRanges != null) i = i + salaryRanges.Count;

                var salaryFrequencies = await _lookupValuesProvider.ReadSalaryFrequencies().ConfigureAwait(false);
                dataSets.AddRange(await CreateDataSetFromLookupValues(indexType, "SalaryFrequency", i, salaryFrequencies, null, null).ConfigureAwait(false));
                if (salaryFrequencies != null) i = i + salaryFrequencies.Count;

                var payGrades = await _lookupValuesProvider.ReadPayGrades().ConfigureAwait(false);
                dataSets.AddRange(await CreateDataSetFromLookupValues(indexType, "PayGrade", i, payGrades, (lookup, query) => query.PayGrades.Add(lookup.Text), jobsDataProvider).ConfigureAwait(false));
                if (payGrades != null) i = i + payGrades.Count;

                var organisations = await _lookupValuesProvider.ReadOrganisations().ConfigureAwait(false);
                dataSets.AddRange(await CreateDataSetFromLookupValues(indexType, "Organisation", i, organisations, (lookup, query) => query.Organisations.Add(lookup.Text), jobsDataProvider).ConfigureAwait(false));
                if (organisations != null) i = i + organisations.Count;

                var workPatterns = await _lookupValuesProvider.ReadWorkPatterns().ConfigureAwait(false);
                dataSets.AddRange(await CreateDataSetFromLookupValues(indexType, "WorkPattern", i, workPatterns, (lookup, query) => query.WorkPatterns.Add(lookup.Text), jobsDataProvider).ConfigureAwait(false));
                if (workPatterns != null) i = i + workPatterns.Count;

                var contractTypes = await _lookupValuesProvider.ReadContractTypes().ConfigureAwait(false);
                dataSets.AddRange(await CreateDataSetFromLookupValues(indexType, "ContractType", i, contractTypes, (lookup, query) => query.ContractTypes.Add(lookup.Text), jobsDataProvider).ConfigureAwait(false));
                if (contractTypes != null) i = i + contractTypes.Count;
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<BaseJobsIndexer>("error indexing:", ex);
            }

            LogHelper.Info<BaseLookupValuesIndexer>($"{dataSets.Count} items built for indexing by {this.GetType().ToString()}");

            return dataSets;
        }

        private static async Task<IEnumerable<SimpleDataSet>> CreateDataSetFromLookupValues(string indexType, string groupKey, int fakeNodeId, IList<JobsLookupValue> lookupValues, Action<JobsLookupValue, JobSearchQuery> addLookupToQuery, IJobsDataProvider jobsDataProvider)
        {
            var dataSets = new List<SimpleDataSet>();
            if (lookupValues != null)
            {
                foreach (var lookupValue in lookupValues)
                {
                    JobSearchQuery query = null;
                    if (addLookupToQuery != null)
                    {
                        query = new JobSearchQuery();
                        addLookupToQuery(lookupValue, query);
                        query.ClosingDateFrom = DateTime.Today;
                    }

                    var simpleDataSet = new SimpleDataSet { NodeDefinition = new IndexedNode(), RowData = new Dictionary<string, string>() };

                    simpleDataSet.NodeDefinition.NodeId = fakeNodeId;
                    simpleDataSet.NodeDefinition.Type = indexType;
                    simpleDataSet.RowData.Add("id", lookupValue.LookupValueId);
                    simpleDataSet.RowData.Add("group", groupKey);
                    simpleDataSet.RowData.Add("text", lookupValue.Text);

                    if (jobsDataProvider != null)
                    {
                        var jobs = await jobsDataProvider.ReadJobs(query).ConfigureAwait(false);
                        simpleDataSet.RowData.Add("count", jobs.TotalJobs.ToString(CultureInfo.CurrentCulture));
                    }

                    dataSets.Add(simpleDataSet);
                }
            }
            return dataSets;
        }
    }
}