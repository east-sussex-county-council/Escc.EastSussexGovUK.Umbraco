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
        public async Task<List<Job>> ReadJobs(JobSearchQuery query)
        {
            if (_lookupValuesParser == null) throw new ArgumentNullException(nameof(_lookupValuesParser));
            if (_jobResultsParser == null) throw new ArgumentNullException(nameof(_jobResultsParser));

            var jobs = new List<Job>();

            var locations = await ReadLocations();
            var jobTypes = await ReadJobTypes();
            var organisations = await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV52");
            var salaryRanges = await ReadSalaryRanges();
            var workPatterns = await ReadWorkPatterns();

            var currentPage = 1;
            while (true)
            {
                var stream = await ReadJobsFromTalentLink(currentPage, query, locations, jobTypes, organisations, salaryRanges, workPatterns);
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

        public async Task<IList<JobsLookupValue>> ReadLocations()
        {
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV39");
        }

        public async Task<IList<JobsLookupValue>> ReadJobTypes()
        {
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV40");
        }

        public async Task<IList<JobsLookupValue>> ReadSalaryRanges()
        {
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV46");
        }
        public async Task<IList<JobsLookupValue>> ReadWorkPatterns()
        {
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV50");
        }

        public async Task<Stream> ReadSearchFieldsHtml()
        {
            return await ReadHtml(_searchUrl, _proxy);
        }

        /// <summary>
        /// Initiates an HTTP request and returns the HTML.
        /// </summary>
        /// <returns></returns>
        private async Task<Stream> ReadJobsFromTalentLink(int currentPage, JobSearchQuery query, IList<JobsLookupValue> locations, IList<JobsLookupValue> jobTypes, IList<JobsLookupValue> organisations, IList<JobsLookupValue> salaryRanges, IList<JobsLookupValue> workPatterns)
        {
            var queryString = HttpUtility.ParseQueryString(_resultsUrl.Query);
            UpdateQueryString(queryString, "resultsperpage", "200");
            UpdateQueryString(queryString, "pagenum", currentPage.ToString(CultureInfo.InvariantCulture));

            if (query != null)
            {
                UpdateQueryString(queryString, "keywords", query.Keywords);
                UpdateQueryString(queryString, "jobnum", query.JobReference);
                AddLookupValueToQueryString(queryString, "LOV39", locations, query.Locations);
                AddLookupValueToQueryString(queryString, "LOV40", jobTypes, query.JobTypes);
                AddLookupValueToQueryString(queryString, "LOV52", organisations, query.Organisations);
                AddLookupValueToQueryString(queryString, "LOV46", salaryRanges, query.SalaryRanges);
                AddLookupValueToQueryString(queryString, "LOV50", workPatterns, query.WorkPatterns);
                AddSortOrderToQueryString(queryString, query.SortBy);
            }

            var pagedSourceUrl = new StringBuilder(_resultsUrl.Scheme).Append("://").Append(_resultsUrl.Authority).Append(_resultsUrl.AbsolutePath).Append("?").Append(queryString);

            return await ReadHtml(new Uri(pagedSourceUrl.ToString()), _proxy);
        }

        private void AddSortOrderToQueryString(NameValueCollection queryString, JobSearchQuery.JobsSortOrder sortBy)
        {
            switch (sortBy)
            {
                case JobSearchQuery.JobsSortOrder.JobTitleAscending:
                    UpdateQueryString(queryString, "option", "21");
                    UpdateQueryString(queryString, "sort", "ASC");
                    break;
                case JobSearchQuery.JobsSortOrder.JobTitleDescending:
                    UpdateQueryString(queryString, "option", "21");
                    UpdateQueryString(queryString, "sort", "DESC");
                    break;
                case JobSearchQuery.JobsSortOrder.OrganisationAscending:
                    UpdateQueryString(queryString, "option", "138");
                    UpdateQueryString(queryString, "sort", "ASC");
                    break;
                case JobSearchQuery.JobsSortOrder.OrganisationDescending:
                    UpdateQueryString(queryString, "option", "138");
                    UpdateQueryString(queryString, "sort", "DESC");
                    break;
                case JobSearchQuery.JobsSortOrder.LocationAscending:
                    UpdateQueryString(queryString, "option", "139");
                    UpdateQueryString(queryString, "sort", "ASC");
                    break;
                case JobSearchQuery.JobsSortOrder.LocationDescending:
                    UpdateQueryString(queryString, "option", "139");
                    UpdateQueryString(queryString, "sort", "DESC");
                    break;
                case JobSearchQuery.JobsSortOrder.SalaryRangeAscending:
                    UpdateQueryString(queryString, "option", "150");
                    UpdateQueryString(queryString, "sort", "ASC");
                    break;
                case JobSearchQuery.JobsSortOrder.SalaryRangeDescending:
                    UpdateQueryString(queryString, "option", "150");
                    UpdateQueryString(queryString, "sort", "DESC");
                    break;
                case JobSearchQuery.JobsSortOrder.ClosingDateAscending:
                    UpdateQueryString(queryString, "option", "48");
                    UpdateQueryString(queryString, "sort", "ASC");
                    break;
                case JobSearchQuery.JobsSortOrder.ClosingDateDescending:
                    UpdateQueryString(queryString, "option", "48");
                    UpdateQueryString(queryString, "sort", "DESC");
                    break;
            }
        }

        private static void AddLookupValueToQueryString(NameValueCollection query, string parameter, IList<JobsLookupValue> lookupValues, IList<string> searchTerms)
        {
            foreach (var searchTerm in searchTerms)
            {
                foreach (var knownValue in lookupValues)
                {
                    if (knownValue.Text?.ToUpperInvariant() == searchTerm?.ToUpperInvariant())
                    {
                        UpdateQueryString(query, parameter, knownValue.Id.ToString(CultureInfo.InvariantCulture), false);
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

        private async Task<IList<JobsLookupValue>> ReadLookupValuesFromTalentLink(IJobLookupValuesParser parser, string fieldName)
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