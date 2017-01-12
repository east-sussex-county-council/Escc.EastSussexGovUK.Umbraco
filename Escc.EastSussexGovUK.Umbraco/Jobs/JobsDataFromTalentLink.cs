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
        private readonly Uri _advertUrl;
        private readonly IProxyProvider _proxy;
        private readonly IJobLookupValuesParser _lookupValuesParser;
        private readonly IJobResultsParser _jobResultsParser;
        private readonly IJobAdvertParser _jobAdvertParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromTalentLink" /> class.
        /// </summary>
        /// <param name="searchUrl">The search URL.</param>
        /// <param name="resultsUrl">The source URL.</param>
        /// <param name="advertUrl">The advert URL.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <param name="lookupValuesParser">The parser for lookup values in the TalentLink HTML.</param>
        /// <param name="jobResultsParser">The job results parser.</param>
        /// <param name="jobAdvertParser">The job advert parser.</param>
        /// <exception cref="System.ArgumentNullException">sourceUrl</exception>
        public JobsDataFromTalentLink(Uri searchUrl, Uri resultsUrl, Uri advertUrl, IProxyProvider proxy, IJobLookupValuesParser lookupValuesParser, IJobResultsParser jobResultsParser, IJobAdvertParser jobAdvertParser)
        {
            _searchUrl = searchUrl;
            _resultsUrl = resultsUrl;
            _advertUrl = advertUrl;
            _proxy = proxy;
            _lookupValuesParser = lookupValuesParser;
            _jobResultsParser = jobResultsParser;
            _jobAdvertParser = jobAdvertParser;
        }


        /// <summary>
        /// Gets the job matching the supplied id
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<Job> ReadJob(string jobId)
        {
            if (String.IsNullOrEmpty(jobId)) return null;
            if (_advertUrl == null) throw new InvalidOperationException("You must specify advertUrl when creating this instance to read an individual job");
            if (_jobAdvertParser == null) throw new InvalidOperationException("You must specify jobAdvertParser when creating this instance to read an individual job");

            var queryString = HttpUtility.ParseQueryString(_advertUrl.Query);
            UpdateQueryString(queryString, "nPostingTargetID", jobId);
            var sourceUrl = new StringBuilder(_advertUrl.Scheme).Append("://").Append(_advertUrl.Authority).Append(_advertUrl.AbsolutePath).Append("?").Append(queryString);
            var htmlStream = await ReadHtml(new Uri(sourceUrl.ToString()), _proxy);

            using (var reader = new StreamReader(htmlStream))
            {
                return _jobAdvertParser.ParseJob(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Reads the jobs from pages of data provided by the <see cref="IJobsDataProvider"/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<Job>> ReadJobs(JobSearchQuery query)
        {
            if (_lookupValuesParser == null) throw new InvalidOperationException("You must specify lookupValuesParser when creating this instance to read jobs");
            if (_jobResultsParser == null) throw new InvalidOperationException("You must specify jobResultsParser when creating this instance to read jobs");

            var jobs = new List<Job>();

            var currentPage = 1;
            while (true)
            {
                var stream = await ReadJobsFromTalentLink(currentPage, query);
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

        /// <summary>
        /// Reads the locations where jobs can be based
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadLocations()
        {
            if (_searchUrl == null) throw new InvalidOperationException("You must specify searchUrl when creating this instance to read locations");
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV39");
        }

        /// <summary>
        /// Reads the job types or categories
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadJobTypes()
        {
            if (_searchUrl == null) throw new InvalidOperationException("You must specify searchUrl when creating this instance to read job types");
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV40");
        }

        /// <summary>
        /// Reads the organisations advertising jobs
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IList<JobsLookupValue>> ReadOrganisations()
        {
            if (_searchUrl == null) throw new InvalidOperationException("You must specify searchUrl when creating this instance to read organisations");
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV52");
        }

        /// <summary>
        /// Reads the salary ranges that jobs can be categorised as
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadSalaryRanges()
        {
            if (_searchUrl == null) throw new InvalidOperationException("You must specify searchUrl when creating this instance to read salary ranges");
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV46");
        }
        /// <summary>
        /// Reads the work patterns, eg full time or part time
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadWorkPatterns()
        {
            if (_searchUrl == null) throw new InvalidOperationException("You must specify searchUrl when creating this instance to read work patterns");
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV50");
        }

        /// <summary>
        /// Initiates an HTTP request and returns the HTML.
        /// </summary>
        /// <returns></returns>
        private async Task<Stream> ReadJobsFromTalentLink(int currentPage, JobSearchQuery query)
        {
            var queryString = HttpUtility.ParseQueryString(_resultsUrl.Query);
            UpdateQueryString(queryString, "resultsperpage", "200");
            UpdateQueryString(queryString, "pagenum", currentPage.ToString(CultureInfo.InvariantCulture));

            if (query != null)
            {
                UpdateQueryString(queryString, "keywords", query.Keywords);
                UpdateQueryString(queryString, "jobnum", query.JobReference);
                UpdateQueryString(queryString, "LOV39", String.Join(",",query.Locations));
                UpdateQueryString(queryString, "LOV40", String.Join(",", query.JobTypes));
                UpdateQueryString(queryString, "LOV52", String.Join(",", query.Organisations));
                UpdateQueryString(queryString, "LOV46", String.Join(",", query.SalaryRanges));
                UpdateQueryString(queryString, "LOV50", String.Join(",", query.WorkPatterns));
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