using Escc.EastSussexGovUK.Umbraco.WebApi;
using Escc.Net;
using Exceptionless;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IJobCacheStrategy _jobsCache;
        private static HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromApi" /> class.
        /// </summary>
        /// <param name="apiBaseUrl">The API base URL.</param>
        /// <param name="jobsSet">The jobs set.</param>
        /// <param name="jobAdvertBaseUrl">The job advert base URL.</param>
        /// <param name="httpClientProvider">A method of getting an <see cref="HttpClient"/> to use for the web API requests</param>
        /// <param name="jobsCache">A method of caching the API results.</param>
        /// <exception cref="ArgumentNullException">apiBaseUrl</exception>
        public JobsDataFromApi(Uri apiBaseUrl, JobsSet jobsSet, Uri jobAdvertBaseUrl, IHttpClientProvider httpClientProvider, IJobCacheStrategy jobsCache)
        {
            _apiBaseUrl = apiBaseUrl ?? throw new ArgumentNullException(nameof(apiBaseUrl));
            _jobsSet = jobsSet;
            _jobAdvertBaseUrl = jobAdvertBaseUrl ?? throw new ArgumentNullException(nameof(jobAdvertBaseUrl));
            _httpClientProvider = httpClientProvider;
            _jobsCache = jobsCache;
        }

        /// <summary>
        /// Gets the job matching the supplied id
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<Job> ReadJob(string jobId)
        {
            if (_jobsCache != null)
            {
                var cached = _jobsCache.ReadJob(jobId);
                if (cached != null) return cached;
            }

            try
            {
                if (_httpClient == null) { _httpClient = _httpClientProvider.GetHttpClient(); }

                var responseJson = await _httpClient.GetStringAsync(new Uri($"{_apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/{_jobsSet}/job/{jobId}/?baseUrl={HttpUtility.UrlEncode(_jobAdvertBaseUrl.ToString())}"));
                var job = JsonConvert.DeserializeObject<Job>(responseJson, new[] { new IHtmlStringConverter() });
                if (_jobsCache != null)
                {
                    _jobsCache.CacheJob(job);
                }
                return job;
            }
            catch (HttpRequestException ex)
            {
                ex.ToExceptionless().Submit();
                return null;
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

            try
            {
                if (_httpClient == null) { _httpClient = _httpClientProvider.GetHttpClient(); }

                var queryString = new JobSearchQueryConverter(ConfigurationManager.AppSettings["TranslateObsoleteJobTypes"]?.ToUpperInvariant() == "TRUE").ToCollection(query).ToString();
                var responseJson = await _httpClient.GetStringAsync(new Uri($"{_apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/{_jobsSet}/jobs/?baseUrl={HttpUtility.UrlEncode(_jobAdvertBaseUrl.ToString())}&{queryString}"));
                var result = JsonConvert.DeserializeObject<JobSearchResult>(responseJson, new[] { new IHtmlStringConverter() });
                if (_jobsCache != null)
                {
                    _jobsCache.CacheJobs(query, result);
                }
                return result;
            }
            catch (HttpRequestException ex)
            {
                ex.ToExceptionless().Submit();
                return new JobSearchResult();
            }
        }

        /// <summary>
        /// Reads the problem jobs.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Job>> ReadProblemJobs()
        {
            try
            {
                if (_httpClient == null) { _httpClient = _httpClientProvider.GetHttpClient(); }

                var responseJson = await _httpClient.GetStringAsync(new Uri($"{_apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/{_jobsSet}/problemjobs/?baseUrl={HttpUtility.UrlEncode(_jobAdvertBaseUrl.ToString())}"));
                return JsonConvert.DeserializeObject<IEnumerable<Job>>(responseJson, new[] { new IHtmlStringConverter() });
            }
            catch (HttpRequestException ex)
            {
                ex.ToExceptionless().Submit();
                return new Job[0];
            }
        }
    }
}