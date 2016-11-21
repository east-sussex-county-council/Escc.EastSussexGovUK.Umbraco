using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Loads jobs data as HTML by making an HTTP request to a TalentLink server
    /// </summary>
    public class JobsDataFromTalentLink : IJobsDataProvider
    {
        private readonly Uri _sourceUrl;
        private readonly IProxyProvider _proxy;
        private readonly IJobResultsParser _parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromTalentLink"/> class.
        /// </summary>
        /// <param name="sourceUrl">The source URL.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <param name="parser">A parser for TalentLink's HTML.</param>
        /// <exception cref="System.ArgumentNullException">sourceUrl</exception>
        public JobsDataFromTalentLink(Uri sourceUrl, IProxyProvider proxy, IJobResultsParser parser)
        {
            if (sourceUrl == null) throw new ArgumentNullException(nameof(sourceUrl));
            if (parser == null) throw new ArgumentNullException(nameof(parser));

            _sourceUrl = sourceUrl;
            _proxy = proxy;
            _parser = parser;
        }


        /// <summary>
        /// Reads the jobs from pages of data provided by the <see cref="IJobsDataProvider"/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<Job>> ReadJobs()
        {
            var jobs = new List<Job>();

            var currentPage = 1;
            while (true)
            {
                var stream = await ReadJobsDataFromTalentLink(currentPage);
                var parseResult = _parser.Parse(stream);

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

        /// <summary>
        /// Initiates an HTTP request and returns the HTML.
        /// </summary>
        /// <returns></returns>
        private async Task<Stream> ReadJobsDataFromTalentLink(int currentPage)
        {
            var handler = new HttpClientHandler()
            {
                Proxy = _proxy?.CreateProxy()
            };

            var pagedSourceUrl = new Uri(_sourceUrl + $"&resultsperpage=200&pagenum={currentPage}");

            var client = new HttpClient(handler);
            return await client.GetStreamAsync(pagedSourceUrl);
        }
    }
}