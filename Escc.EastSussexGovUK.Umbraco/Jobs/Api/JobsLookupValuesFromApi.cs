using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Api
{
    /// <summary>
    /// Reads jobs data from a web API which derives from <see cref="BaseJobsApiController"/>
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IJobsLookupValuesProvider" />
    public class JobsLookupValuesFromApi : IJobsLookupValuesProvider
    {
        private readonly Uri _apiBaseUrl;
        private readonly JobsSet _jobsSet;

        /// <summary>
        /// Jobses the lookup values from API.
        /// </summary>
        /// <param name="apiBaseUrl">The API base URL.</param>
        /// <param name="jobsSet">The jobs set.</param>
        /// <exception cref="ArgumentNullException">apiBaseUrl</exception>
        public JobsLookupValuesFromApi(Uri apiBaseUrl, JobsSet jobsSet)
        {
            this._apiBaseUrl = apiBaseUrl ?? throw new ArgumentNullException(nameof(apiBaseUrl));
            this._jobsSet = jobsSet;
        }

        /// <summary>
        /// Reads the job types or categories
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadJobTypes()
        {
            return await ReadLookupValuesFromApi("jobtypes");
        }

        /// <summary>
        /// Reads the locations where jobs can be based
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadLocations()
        {
            return await ReadLookupValuesFromApi("locations");
        }

        /// <summary>
        /// Reads the organisations advertising jobs
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadOrganisations()
        {
            return await ReadLookupValuesFromApi("organisations");
        }

        /// <summary>
        /// Reads the salary ranges that jobs can be categorised as
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadSalaryRanges()
        {
            return await ReadLookupValuesFromApi("salaryranges");
        }

        /// <summary>
        /// Reads the work patterns, eg full time or part time
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadWorkPatterns()
        {
            return await ReadLookupValuesFromApi("workpatterns");
        }

        private async Task<IList<JobsLookupValue>> ReadLookupValuesFromApi(string apiMethod)
        {
            var request = WebRequest.Create(new Uri($"{_apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/{_jobsSet}/{apiMethod}/"));
            using (var response = await request.GetResponseAsync())
            {
                using (var responseReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseJson = responseReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<IList<JobsLookupValue>>(responseJson);
                }
            }
        }
    }
}