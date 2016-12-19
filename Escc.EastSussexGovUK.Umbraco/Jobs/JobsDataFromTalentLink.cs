using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Loads jobs data as HTML by making an HTTP request to a TalentLink server
    /// </summary>
    public class JobsDataFromTalentLink : IJobsDataProvider
    {
        private readonly Uri _searchUrl;
        private readonly Uri _resultsUrl;
        private readonly IProxyProvider _proxy;
        private readonly IJobTypesParser _jobTypesParser;
        private readonly IJobResultsParser _jobResultsParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromTalentLink" /> class.
        /// </summary>
        /// <param name="searchUrl">The search URL.</param>
        /// <param name="resultsUrl">The source URL.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <param name="jobTypesParser">The job types parser.</param>
        /// <param name="jobResultsParser">The job results parser.</param>
        /// <exception cref="System.ArgumentNullException">sourceUrl</exception>
        public JobsDataFromTalentLink(Uri searchUrl, Uri resultsUrl, IProxyProvider proxy, IJobTypesParser jobTypesParser, IJobResultsParser jobResultsParser)
        {
            if (searchUrl == null) throw new ArgumentNullException(nameof(searchUrl));
            if (resultsUrl == null) throw new ArgumentNullException(nameof(resultsUrl));

            _searchUrl = searchUrl;
            _resultsUrl = resultsUrl;
            _proxy = proxy;
            _jobTypesParser = jobTypesParser;
            _jobResultsParser = jobResultsParser;
        }


        /// <summary>
        /// Reads the jobs from pages of data provided by the <see cref="IJobsDataProvider"/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<Job>> ReadJobs(JobSearchFilter filter)
        {
            if (_jobTypesParser == null) throw new ArgumentNullException(nameof(_jobTypesParser));
            if (_jobResultsParser == null) throw new ArgumentNullException(nameof(_jobResultsParser));

            var jobs = new List<Job>();

            var jobTypes = await ReadJobTypesFromTalentLink(_jobTypesParser);

            var currentPage = 1;
            while (true)
            {
                var stream = await ReadJobsFromTalentLink(currentPage, filter, jobTypes);
                var parseResult = _jobResultsParser.Parse(stream);

                if (parseResult.Jobs.Count > 0)
                {
                    jobs.AddRange(parseResult.Jobs);
                }

                if (parseResult.IsLastPage)
                {
                    break;
                }
                else
                {
                    currentPage++;
                }
            }
            return jobs;
        }

        public async Task<Stream> ReadSearchFieldsHtml()
        {
            return await ReadHtml(_searchUrl, _proxy);
        }

        /// <summary>
        /// Initiates an HTTP request and returns the HTML.
        /// </summary>
        /// <returns></returns>
        private async Task<Stream> ReadJobsFromTalentLink(int currentPage, JobSearchFilter filter, Dictionary<int,string> jobTypes)
        {
            var pagedSourceUrl = new StringBuilder(_resultsUrl.ToString()).Append($"&resultsperpage=200&pagenum={currentPage}");
            if (filter != null)
            {
                foreach (var jobType in filter.JobTypes)
                {
                    foreach (var knownJobType in jobTypes)
                    {
                        if (knownJobType.Value == jobType)
                        {
                            pagedSourceUrl.Append("&LOV40=").Append(HttpUtility.UrlEncode(knownJobType.Key.ToString(CultureInfo.InvariantCulture)));
                        }
                    }
                }
            }

            return await ReadHtml(new Uri(pagedSourceUrl.ToString()), _proxy);
        }

        private async Task<Dictionary<int,string>> ReadJobTypesFromTalentLink(IJobTypesParser parser)
        {
            var htmlStream = await ReadHtml(_searchUrl, _proxy);

            using (var reader = new StreamReader(htmlStream))
            {
                return parser.ParseJobTypes(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Initiates an HTTP request and returns the HTML.
        /// </summary>
        /// <returns></returns>
        private static async Task<Stream> ReadHtml(Uri url, IProxyProvider proxy)
        {
            var handler = new HttpClientHandler()
            {
                Proxy = proxy?.CreateProxy()
            };
            var client = new HttpClient(handler);
            return await client.GetStreamAsync(url);
        }
    }
}