﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Examine;
using Examine.LuceneEngine.Providers;
using Exceptionless;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Examine;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Examine
{
    [Authorize]
    public class JobsIndexerApiController : UmbracoApiController
    {
        /// <summary>
        /// Updates the job search by deleting and rebuilding the index from scratch. Search results are unavailable while this is in progress.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UpdateJobSearch()
        {
            try
            {
                ReplaceIndex("PublicJobsIndexer");
                ReplaceIndex("RedeploymentJobsIndexer");
                ReplaceIndex("PublicJobsLookupValuesIndexer");
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

        /// <summary>
        /// Updates the job search by deleting and reindexing each job individually. Search results remain available while this is in progress.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UpdateJobSearchIncremental()
        {
            try
            {
                UpdateIndex("PublicJobsSearcher", new PublicJobsIndexer());
                UpdateIndex("RedeploymentJobsSearcher", new RedeploymentJobsIndexer());
                ReplaceIndex("PublicJobsLookupValuesIndexer");
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

        private static void UpdateIndex(string searcherName, BaseJobsIndexer indexer)
        {
            var jobsSearcher = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[searcherName], null, null, null);
            var jobs = jobsSearcher.ReadJobs(new JobSearchQuery()).Result;
            var jobIds = new Dictionary<int,DateTime?>();
            foreach (var job in jobs.Jobs)
            {
                if (!jobIds.ContainsKey(job.Id)) jobIds.Add(job.Id, job.DatePublished);
            }

            indexer.UpdateIndex(jobIds);
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