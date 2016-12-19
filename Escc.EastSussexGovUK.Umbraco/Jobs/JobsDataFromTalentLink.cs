using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private readonly IJobLookupValuesParser _lookupValuesParser;
        private readonly IJobResultsParser _jobResultsParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromTalentLink" /> class.
        /// </summary>
        /// <param name="searchUrl">The search URL.</param>
        /// <param name="resultsUrl">The source URL.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <param name="lookupValuesParser">The parser for lookup values in the TalentLink HTML.</param>
        /// <param name="jobResultsParser">The job results parser.</param>
        /// <exception cref="System.ArgumentNullException">sourceUrl</exception>
        public JobsDataFromTalentLink(Uri searchUrl, Uri resultsUrl, IProxyProvider proxy, IJobLookupValuesParser lookupValuesParser, IJobResultsParser jobResultsParser)
        {
            if (searchUrl == null) throw new ArgumentNullException(nameof(searchUrl));
            if (resultsUrl == null) throw new ArgumentNullException(nameof(resultsUrl));

            _searchUrl = searchUrl;
            _resultsUrl = resultsUrl;
            _proxy = proxy;
            _lookupValuesParser = lookupValuesParser;
            _jobResultsParser = jobResultsParser;
        }


        /// <summary>
        /// Reads the jobs from pages of data provided by the <see cref="IJobsDataProvider"/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<Job>> ReadJobs(JobSearchFilter filter)
        {
            if (_lookupValuesParser == null) throw new ArgumentNullException(nameof(_lookupValuesParser));
            if (_jobResultsParser == null) throw new ArgumentNullException(nameof(_jobResultsParser));

            var jobs = new List<Job>();

            var locations = await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV39");
            var jobTypes = await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV40");
            var organisations = await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV52");
            var salaryRanges = await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV46");
            var workingHours = await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV50");

            var currentPage = 1;
            while (true)
            {
                var stream = await ReadJobsFromTalentLink(currentPage, filter, locations, jobTypes, organisations, salaryRanges, workingHours);
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
        private async Task<Stream> ReadJobsFromTalentLink(int currentPage, JobSearchFilter filter, Dictionary<int,string> locations, Dictionary<int,string> jobTypes, Dictionary<int,string> organisations, Dictionary<int,string> salaryRanges, Dictionary<int,string> workingHours)
        {
            var query = HttpUtility.ParseQueryString(_resultsUrl.Query);
            UpdateQueryString(query, "resultsperpage", "200");
            UpdateQueryString(query, "pagenum", currentPage.ToString(CultureInfo.InvariantCulture));

            if (filter != null)
            {
                UpdateQueryString(query, "keywords", filter.Keywords);
                UpdateQueryString(query, "jobnum", filter.JobReference);
                AddLookupValueToQueryString(query, "LOV39", locations, new [] { filter.Location});
                AddLookupValueToQueryString(query, "LOV40", jobTypes, filter.JobTypes);
                AddLookupValueToQueryString(query, "LOV52", organisations, new [] { filter.Organisation});
                AddLookupValueToQueryString(query, "LOV46", salaryRanges, new [] { filter.SalaryRange});
                AddLookupValueToQueryString(query, "LOV50", workingHours, filter.WorkingHours);
            }

            var pagedSourceUrl = new StringBuilder(_resultsUrl.Scheme).Append("://").Append(_resultsUrl.Authority).Append(_resultsUrl.AbsolutePath).Append("?").Append(query);

            return await ReadHtml(new Uri(pagedSourceUrl.ToString()), _proxy);
        }

        private static void AddLookupValueToQueryString(NameValueCollection query, string parameter, Dictionary<int, string> lookupValues, IList<string> searchTerms)
        {
            foreach (var searchTerm in searchTerms)
            {
                foreach (var knownValue in lookupValues)
                {
                    if (knownValue.Value?.ToUpperInvariant() == searchTerm?.ToUpperInvariant())
                    {
                        UpdateQueryString(query, parameter, knownValue.Key.ToString(CultureInfo.InvariantCulture), false);
                    }
                }
            }
        }

        private static void UpdateQueryString(NameValueCollection query, string parameter, string value, bool replaceExistingValue=true)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (replaceExistingValue) query.Remove(parameter);
                query.Add(parameter, HttpUtility.UrlEncode(value));
            }
        }

        private async Task<Dictionary<int,string>> ReadLookupValuesFromTalentLink(IJobLookupValuesParser parser, string fieldName)
        {
            var htmlStream = await ReadHtml(_searchUrl, _proxy);

            using (var reader = new StreamReader(htmlStream))
            {
                return parser.ParseLookupValues(reader.ReadToEnd(), fieldName);
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