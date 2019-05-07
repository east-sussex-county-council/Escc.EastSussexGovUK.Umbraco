using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Escc.Net;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// Loads jobs data as HTML by making an HTTP request to a TribePad server
    /// </summary>
    public class JobsDataFromTribePad : IJobsDataProvider
    {
        private readonly IEnumerable<Uri> _resultsUrls;
        private readonly Uri _advertUrl;
        private readonly IProxyProvider _proxy;
        private readonly IJobResultsParser _jobResultsParser;
        private readonly IJobAdvertParser _jobAdvertParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromTribePad" /> class.
        /// </summary>
        /// <param name="resultsUrls">One or more source URLs.</param>
        /// <param name="advertUrl">The advert URL.</param>
        /// <param name="jobResultsParser">The job results parser.</param>
        /// <param name="jobAdvertParser">The job advert parser.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <exception cref="System.ArgumentNullException">sourceUrl</exception>
        public JobsDataFromTribePad(IEnumerable<Uri> resultsUrls, Uri advertUrl, IJobResultsParser jobResultsParser, IJobAdvertParser jobAdvertParser, IProxyProvider proxy)
        {
            _resultsUrls = resultsUrls;
            _advertUrl = advertUrl;
            _proxy = proxy;
            _jobResultsParser = jobResultsParser;
            _jobAdvertParser = jobAdvertParser;
        }


        /// <summary>
        /// Gets the job matching the supplied id
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<Job> ReadJob(string jobId)
        {
            if (String.IsNullOrEmpty(jobId)) return null;
            if (_advertUrl == null) throw new InvalidOperationException("You must specify advertUrl when creating this instance to read an individual job");
            if (_jobAdvertParser == null) throw new InvalidOperationException("You must specify jobAdvertParser when creating this instance to read an individual job");

            var advertUrl = new Uri(String.Format(CultureInfo.InvariantCulture, _advertUrl.ToString(), jobId));

            var stream = await ReadXml(advertUrl, _proxy);

            using (var reader = new StreamReader(stream))
            {
                return await _jobAdvertParser.ParseJob(reader.ReadToEnd(), jobId);
            }
        }

        /// <summary>
        /// Reads the jobs from pages of data provided by the <see cref="IJobsDataProvider"/>
        /// </summary>
        /// <returns></returns>
        public async Task<JobSearchResult> ReadJobs(JobSearchQuery query)
        {
            if (_jobResultsParser == null) throw new InvalidOperationException("You must specify jobResultsParser when creating this instance to read jobs");

            var jobs = new JobSearchResult() { Jobs = new List<Job>() };

            foreach (var resultsUrl in _resultsUrls)
            {
                var stream = await ReadXml(resultsUrl, _proxy).ConfigureAwait(false);
                jobs.Jobs.AddRange((await _jobResultsParser.Parse(stream)).Jobs);
            }
            return jobs;
        }


        /// <summary>
        /// Initiates an HTTP request and returns the XML.
        /// </summary>
        /// <returns></returns>
        private static async Task<Stream> ReadXml(Uri url, IProxyProvider proxy)
        {
            var handler = new HttpClientHandler()
            {
                Proxy = proxy?.CreateProxy()
            };
            var client = new HttpClient(handler);
            return await client.GetStreamAsync(url).ConfigureAwait(false);
        }
    }
}