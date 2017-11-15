using Escc.EastSussexGovUK.Umbraco.WebApi;
using Escc.Net;
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
    /// Reads jobs data from a web API which derives from <see cref="BaseJobsApiControlller"/>
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IJobsDataProvider" />
    public class JobsDataFromApi : IJobsDataProvider
    {
        private readonly Uri _apiBaseUrl;
        private readonly JobsSet _jobsSet;
        private readonly Uri _jobAdvertBaseUrl;
        private readonly IJobCacheStrategy _jobsCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromApi" /> class.
        /// </summary>
        /// <param name="apiBaseUrl">The API base URL.</param>
        /// <param name="jobsSet">The jobs set.</param>
        /// <param name="jobAdvertBaseUrl">The job advert base URL.</param>
        /// <param name="jobsCache">A method of caching the API results.</param>
        /// <exception cref="ArgumentNullException">apiBaseUrl</exception>
        public JobsDataFromApi(Uri apiBaseUrl, JobsSet jobsSet, Uri jobAdvertBaseUrl, IJobCacheStrategy jobsCache)
        {
            this._apiBaseUrl = apiBaseUrl ?? throw new ArgumentNullException(nameof(apiBaseUrl));
            this._jobsSet = jobsSet;
            this._jobAdvertBaseUrl = jobAdvertBaseUrl ?? throw new ArgumentNullException(nameof(jobAdvertBaseUrl));
            this._jobsCache = jobsCache;
        }

        /// <summary>
        /// Gets the job matching the supplied id
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<Job> ReadJob(string jobId)
        {
            if (_jobsCache !=null)
            {
                var cached = _jobsCache.ReadJob(jobId);
                if (cached != null) return cached;
            }

            var request = WebRequest.Create(new Uri($"{_apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/{_jobsSet}/job/{jobId}/?baseUrl={HttpUtility.UrlEncode(_jobAdvertBaseUrl.ToString())}"));
            using (var response = await request.GetResponseAsync())
            {
                using (var responseReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseJson = responseReader.ReadToEnd();
                    var job = JsonConvert.DeserializeObject<Job>(responseJson, new[] { new IHtmlStringConverter() });
                    if (_jobsCache !=null)
                    {
                        _jobsCache.CacheJob(job);
                    }
                    return job;
                }
            }
        }

        /// <summary>
        /// Gets the jobs matching the supplied query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<JobSearchResult> ReadJobs(JobSearchQuery query)
        {
            if (_jobsCache != null)
            {
                var cached = _jobsCache.ReadJobs(query);
                if (cached != null) return cached;
            }

            var queryString = new JobSearchQueryConverter().ToCollection(query).ToString();
            var request = WebRequest.Create(new Uri($"{_apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/{_jobsSet}/jobs/?baseUrl={HttpUtility.UrlEncode(_jobAdvertBaseUrl.ToString())}&{queryString}"));
            using (var response = await request.GetResponseAsync())
            {
                using (var responseReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseJson = responseReader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<JobSearchResult>(responseJson, new[] { new IHtmlStringConverter() });
                    if (_jobsCache != null)
                    {
                        _jobsCache.CacheJobs(query, result);
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// Reads the problem jobs.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Job>> ReadProblemJobs()
        {
            var request = WebRequest.Create(new Uri($"{_apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/{_jobsSet}/problemjobs/?baseUrl={HttpUtility.UrlEncode(_jobAdvertBaseUrl.ToString())}"));
            using (var response = await request.GetResponseAsync())
            {
                using (var responseReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseJson = responseReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<IEnumerable<Job>>(responseJson, new[] { new IHtmlStringConverter() });
                }
            }
        }
    }
}