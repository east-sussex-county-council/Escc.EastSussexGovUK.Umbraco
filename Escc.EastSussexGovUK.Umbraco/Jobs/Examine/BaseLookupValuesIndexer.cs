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

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Examine
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
                var lookupValues = Task.Run(async () => await _lookupValuesProvider.ReadSalaryRanges()).Result;

                var i = 1;
                foreach (var lookupValue in lookupValues)
                {
                    var simpleDataSet = new SimpleDataSet { NodeDefinition = new IndexedNode(), RowData = new Dictionary<string, string>() };

                    simpleDataSet.NodeDefinition.NodeId = i;
                    simpleDataSet.NodeDefinition.Type = indexType;
                    simpleDataSet.RowData.Add("group", "SalaryRange");
                    simpleDataSet.RowData.Add("text", lookupValue.Text);

                    if (jobsDataProvider != null)
                    {
                        var query = new JobSearchQuery();
                        query.SalaryRanges.Add(lookupValue.Text);
                        var jobs = Task.Run(async () => await jobsDataProvider.ReadJobs(query)).Result;
                        simpleDataSet.RowData.Add("count", jobs.Count.ToString(CultureInfo.CurrentCulture));
                    }

                    dataSets.Add(simpleDataSet);
                    i++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<BaseJobsIndexer>("error indexing:", ex);
            }

            return dataSets;
        }
    }
}