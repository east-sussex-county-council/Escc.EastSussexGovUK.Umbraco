using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Escc.Net;
using Escc.Dates;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink
{
    /// <summary>
    /// Loads jobs data as HTML by making an HTTP request to a TalentLink server
    /// </summary>
    public class JobsDataFromTalentLink : IJobsDataProvider
    {
        private readonly Uri _resultsUrl;
        private readonly Uri _advertUrl;
        private readonly IProxyProvider _proxy;
        private readonly IJobResultsParser _jobResultsParser;
        private readonly IJobAdvertParser _jobAdvertParser;
        private readonly bool _saveHtml;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromTalentLink" /> class.
        /// </summary>
        /// <param name="resultsUrl">The source URL.</param>
        /// <param name="advertUrl">The advert URL.</param>
        /// <param name="jobResultsParser">The job results parser.</param>
        /// <param name="jobAdvertParser">The job advert parser.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <param name="saveHtml">Save a copy of the TalentLink HTML to App_Data</param>
        /// <exception cref="System.ArgumentNullException">sourceUrl</exception>
        public JobsDataFromTalentLink(Uri resultsUrl, Uri advertUrl, IJobResultsParser jobResultsParser, IJobAdvertParser jobAdvertParser, IProxyProvider proxy, bool saveHtml)
        {
            _resultsUrl = resultsUrl;
            _advertUrl = advertUrl;
            _proxy = proxy;
            _jobResultsParser = jobResultsParser;
            _jobAdvertParser = jobAdvertParser;
            _saveHtml = saveHtml;
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

            var currentPage = 1;
            while (true)
            {
                Stream stream = null;
                try
                {
                    stream = await ReadJobsFromTalentLink(currentPage, query).ConfigureAwait(false);

                    if (_saveHtml)
                    {
                        stream = SaveHtml(query, currentPage, stream);
                    }

                    var parseResult = await _jobResultsParser.Parse(stream).ConfigureAwait(false);

                    if (parseResult.Jobs.Count > 0)
                    {
                        jobs.Jobs.AddRange(parseResult.Jobs);
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
                finally
                {
                    stream?.Dispose();
                }
            }
            return jobs;
        }

        /// <summary>
        /// Saves the HTML to a folder under App_Data so that the success of the import can be monitored
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        private Stream SaveHtml(JobSearchQuery query, int currentPage, Stream stream)
        {
            var folder = HostingEnvironment.MapPath("~/App_Data/TalentLinkHtml");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string html = null;
            using (var reader = new StreamReader(stream))
            {
                var queryString = HttpUtility.ParseQueryString(_resultsUrl.Query);
                var htmlFilename = $"{folder}/{queryString["mask"]}-{DateTime.UtcNow.ToIso8601DateTime().Replace(":", "-")}-{query.ToHash()}-Page{currentPage}.html";
                html = reader.ReadToEnd();
                File.WriteAllText(htmlFilename, html);
            }
            stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            return stream;
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

            return await ReadHtml(new Uri(pagedSourceUrl.ToString()), _proxy).ConfigureAwait(false);
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
            return await client.GetStreamAsync(url).ConfigureAwait(false);
        }
    }
}