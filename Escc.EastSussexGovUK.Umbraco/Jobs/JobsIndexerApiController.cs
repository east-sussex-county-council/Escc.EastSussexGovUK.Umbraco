using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.UI.WebControls;
using Examine;
using Examine.LuceneEngine.Providers;
using Exceptionless;
using Exceptionless.Models;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;
using Umbraco.Web.WebServices;
using System.Web.Http;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
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

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                LogHelper.Error<JobsIndexerApiController>("Failed to reindex jobs. There may be another thread currently writing to the index.", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private static void UpdateIndex(string indexerName)
        {
            DateTime startTime = DateTime.Now;
            var indexer = ExamineManager.Instance.IndexProviderCollection[indexerName] as LuceneIndexer;
            indexer?.RebuildIndex();
            var duration = DateTime.Now.Subtract(startTime).TotalSeconds;

            LogHelper.Info<JobsIndexerApiController>($"Examine index '{indexerName}' rebuilt successfully in {duration} seconds from a call to JobsIndexerApiController");
        }
    }
}