using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Examine;
using Examine.LuceneEngine.Providers;
using Exceptionless;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using System.Xml.Linq;
using System.Web.Hosting;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine
{
    [Authorize]
    public class JobsIndexerApiController : UmbracoApiController
    {
        /// <summary>
        /// Updates the public job search by deleting and reindexing each job individually. Search results remain available while this is in progress.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> UpdatePublicJobs()
        {
            try
            {
                await UpdateIndex("PublicJobsIndexer", "PublicJobsSearcher");
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<JobsIndexerApiController>("Failed to reindex jobs. There may be another thread currently writing to the index.", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Updates the redeployment job search by deleting and reindexing each job individually. Search results remain available while this is in progress.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> UpdateRedeploymentJobs()
        {
            try
            {
                await UpdateIndex("RedeploymentJobsIndexer", "RedeploymentJobsSearcher");
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<JobsIndexerApiController>("Failed to reindex jobs. There may be another thread currently writing to the index.", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Updates the public job search lookup values by reading data from the indexed jobs.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ReplacePublicJobsLookupValues()
        {
            try
            {
                ReplaceIndex("PublicJobsLookupValuesIndexer");
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<JobsIndexerApiController>("Failed to reindex jobs. There may be another thread currently writing to the index.", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Updates the redeployment job search lookup values by reading data from the indexed jobs.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ReplaceRedeploymentJobsLookupValues()
        {
            try
            {
                ReplaceIndex("RedeploymentJobsLookupValuesIndexer");
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<JobsIndexerApiController>("Failed to reindex jobs. There may be another thread currently writing to the index.", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private async static Task UpdateIndex(string indexerName, string searcherName)
        {
            var jobsSearcher = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[searcherName], null, null, null);
            var jobs = await jobsSearcher.ReadJobs(new JobSearchQuery());
            var jobIds = new Dictionary<int,DateTime?>();
            foreach (var job in jobs.Jobs)
            {
                if (!jobIds.ContainsKey(job.Id)) jobIds.Add(job.Id, job.DatePublished);
            }

            var examineConfig = XDocument.Load(HostingEnvironment.MapPath("~/config/ExamineSettings.config"));
            var indexerConfig = examineConfig.Root.Element("ExamineIndexProviders").Element("providers").Elements("add").SingleOrDefault(x => x.Attribute("name").Value == indexerName);
            if (indexerConfig != null)
            {
                var indexer = (BaseJobsIndexer)Activator.CreateInstance(Type.GetType(indexerConfig.Attribute("dataService").Value));
                indexer.UpdateIndex(jobIds);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> UpdateJobsLookupValues()
        {
            try
            {
                // If the lookups index is built before the jobs index then it will be be full of 0 values.
                // This happens often when Azure moves the site to a new machine and the indexes are rebuilt in parallel,
                // so provide a way to check for missing values and rebuild the lookups index
                await UpdateIndexIfNoLookups("PublicJobsLookupValues");
                await UpdateIndexIfNoLookups("RedeploymentJobsLookupValues");

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<JobsIndexerApiController>("Failed to reindex jobs. There may be another thread currently writing to the index.", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private static async Task UpdateIndexIfNoLookups(string indexPrefix)
        {
            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[indexPrefix + "Searcher"]);
            var lookupValues = await dataSource.ReadJobTypes();
            var jobFound = false;
            foreach (var lookupValue in lookupValues)
            {
                if (lookupValue.Count > 0)
                {
                    jobFound = true;
                    break;
                }
            }
            if (!jobFound) ReplaceIndex(indexPrefix + "Indexer");
        }

        private static void ReplaceIndex(string indexerName)
        {
            LogHelper.Info<JobsIndexerApiController>($"Beginning rebuild of Examine index '{indexerName}' from a call to JobsIndexerApiController");

            DateTime startTime = DateTime.Now;
            var indexer = ExamineManager.Instance.IndexProviderCollection[indexerName] as LuceneIndexer;
            indexer?.RebuildIndex();
            var duration = DateTime.Now.Subtract(startTime).TotalSeconds;

            LogHelper.Info<JobsIndexerApiController>($"Examine index '{indexerName}' rebuilt successfully in {duration} seconds from a call to JobsIndexerApiController");
        }
    }
}