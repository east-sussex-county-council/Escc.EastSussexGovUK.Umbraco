using Exceptionless;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Api
{
    /// <summary>
    /// API for job adverts in the <see cref="JobsSet.PublicJobs"/> jobs set
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.Api.BaseJobsApiController" />
    public class PublicJobsController : BaseJobsApiController
    {
        // GET /umbraco/api/publicjobs/jobs/
        [HttpGet]
        public JobSearchResult Jobs([FromUri] string baseUrl)
        {
            try
            {
                return base.Jobs(JobsSet.PublicJobs, new JobSearchQueryConverter().ToQuery(HttpUtility.ParseQueryString(Request.RequestUri.Query)), new Uri(baseUrl));
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return new JobSearchResult();
            }
        }

        // GET /umbraco/api/publicjobs/problemjobs/
        [HttpGet]
        public async Task<IEnumerable<Job>> ProblemJobs([FromUri] string baseUrl)
        {
            try
            {
                return await base.ProblemJobs(JobsSet.PublicJobs, new Uri(baseUrl));
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return new Job[0];
            }
        }

        // GET /umbraco/api/publicjobs/job/{id}
        [HttpGet]
        public async Task<Job> Job(string id, [FromUri] string baseUrl)
        {
            try
            {
                return await base.Job(JobsSet.PublicJobs, id, new Uri(baseUrl));
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return null;
            }
        }
    }
}