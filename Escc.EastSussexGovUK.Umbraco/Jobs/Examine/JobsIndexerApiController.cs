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

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Examine
{
    [Authorize]
    public class JobsIndexerApiController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage UpdateJobSearch()
        {
            try
            {
                UpdateIndex("PublicJobsIndexer");
                UpdateIndex("RedeploymentJobsIndexer");
                UpdateIndex("PublicJobsLookupValuesIndexer");
                UpdateIndex("RedeploymentJobsLookupValuesIndexer");

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<JobsIndexerApiController>("Failed to reindex jobs. There may be another thread currently writing to the index.", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public HttpResponseMessage UpdateJobsLookupValues()
        {
            try
            {
                // If the lookups index is built before the jobs index then it will be be full of 0 values.
                // This happens often when Azure moves the site to a new machine and the indexes are rebuilt in parallel,
                // so provide a way to check for missing values and rebuild the lookups index
                UpdateIndexIfNoLookups("PublicJobsLookupValues");
                UpdateIndexIfNoLookups("RedeploymentJobsLookupValues");

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<JobsIndexerApiController>("Failed to reindex jobs. There may be another thread currently writing to the index.", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private static void UpdateIndexIfNoLookups(string indexPrefix)
        {
            var dataSource = new JobsLookupValuesFromExamine(ExamineManager.Instance.SearchProviderCollection[indexPrefix + "Searcher"]);
            var lookupValues = Task.Run(async () => await dataSource.ReadJobTypes());
            var jobFound = false;
            foreach (var lookupValue in lookupValues.Result)
            {
                if (lookupValue.Count > 0)
                {
                    jobFound = true;
                    break;
                }
            }
            if (!jobFound) UpdateIndex(indexPrefix + "Indexer");
        }

        private static void UpdateIndex(string indexerName)
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