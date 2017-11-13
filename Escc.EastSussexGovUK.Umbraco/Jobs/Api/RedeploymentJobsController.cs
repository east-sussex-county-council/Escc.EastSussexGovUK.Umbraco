using Exceptionless;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Api
{
    /// <summary>
    /// API for job adverts in the <see cref="JobsSet.RedeploymentJobs"/> jobs set
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.Api.BaseJobsApiController" />
    public class RedeploymentJobsController : BaseJobsApiController
    {
        // GET /umbraco/api/redeploymentjobs/jobs/
        [HttpGet]
        public JobSearchResult Jobs([FromUri] string baseUrl)
        {
            try
            {
                return base.Jobs(JobsSet.RedeploymentJobs, new JobSearchQueryConverter().ToQuery(HttpUtility.ParseQueryString(Request.RequestUri.Query)), new Uri(baseUrl));
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return new JobSearchResult();
            }
        }


        // GET /umbraco/api/redeploymentjobs/problemjobs/
        [HttpGet]
        public async Task<IEnumerable<Job>> ProblemJobs([FromUri] string baseUrl)
        {
            try
            {
                return await base.ProblemJobs(JobsSet.RedeploymentJobs, new Uri(baseUrl));
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return new Job[0];
            }
        }

        // GET /umbraco/api/redeploymentjobs/job/{id}
        [HttpGet]
        public async Task<Job> Job(string id, [FromUri] string baseUrl)
        {
            try
            {
                return await base.Job(JobsSet.RedeploymentJobs, id, new Uri(baseUrl));
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return null;
            }
        }
    }
}