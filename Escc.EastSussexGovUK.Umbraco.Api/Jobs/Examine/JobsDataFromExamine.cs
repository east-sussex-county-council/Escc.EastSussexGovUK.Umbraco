using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Examine;
using Examine.SearchCriteria;
using Exceptionless;
using log4net;
using Lucene.Net.QueryParsers;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine
{
    /// <summary>
    /// Reads jobs from the Examine index
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IJobsDataProvider" />
    public class JobsDataFromExamine : IJobsDataProvider
    {
        private readonly ISearcher _searcher;
        private readonly IQueryBuilder _keywordsQueryBuilder;
        private readonly ISalaryRangeQueryBuilder _salaryQueryBuilder;
        private readonly IJobUrlGenerator _urlGenerator;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromExamine" /> class.
        /// </summary>
        /// <param name="searcher">The searcher.</param>
        /// <param name="keywordsQueryBuilder">The query analyser.</param>
        /// <param name="jobAdvertUrl">The job advert URL.</param>
        /// <exception cref="System.ArgumentNullException">searcher</exception>
        public JobsDataFromExamine(ISearcher searcher, IQueryBuilder keywordsQueryBuilder, ISalaryRangeQueryBuilder salaryQueryBuilder, IJobUrlGenerator urlGenerator)
        {
            if (searcher == null) throw new ArgumentNullException(nameof(searcher));
            _searcher = searcher;
            _keywordsQueryBuilder = keywordsQueryBuilder;
            _salaryQueryBuilder = salaryQueryBuilder;
            _urlGenerator = urlGenerator;
        }

        /// <summary>
        /// Reads a job by its id.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns></returns>
        public Task<Job> ReadJob(string jobId)
        {
            var results = _searcher.Search(_searcher.CreateSearchCriteria().Field("id", jobId).Compile());
            var jobs = BuildJobsFromExamineResults(results);
            if (jobs.Count > 0)
            {
                return Task.FromResult(jobs[0]);
            }
            else
            {
                return Task.FromResult(new Job());
            }
        }

        /// <summary>
        /// Gets the jobs matching the supplied query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<JobSearchResult> ReadJobs(JobSearchQuery query)
        {
            var examineQuery = _searcher.CreateSearchCriteria(BooleanOperation.And);

            examineQuery.GroupedOr(new[] { "location" }, query.Locations.ToArray())
                        .And().GroupedOr(new[] { "contractType"}, query.ContractTypes.ToArray())
                        .And().GroupedOr(new[] { "workPattern"}, query.WorkPatterns.ToArray())
                        .And().GroupedOr(new[] { "jobType" }, query.JobTypes.ToArray())
                        .And().GroupedOr(new[] { "organisation" }, query.Organisations.ToArray());

            // If any of the GroupedOr values are empty, Examine generates a Lucene query that requires no field set to no value: +()
            // so we need to remove that and use the rest of the generated Lucene query
            var generatedQuery = examineQuery.ToString();
            generatedQuery = generatedQuery.Substring(34, generatedQuery.Length - 36);
            var modifiedQuery = generatedQuery.Replace("+()", String.Empty);

            if (_keywordsQueryBuilder != null)
            {
                // Search for title keywords by building up a clause that looks for any of the keywords to be present 
                // in the job title
                modifiedQuery += _keywordsQueryBuilder.AnyOfTheseTermsInThisField(query.KeywordsInTitle ?? String.Empty, new SearchField() { FieldName = "title" }, true);

                // Search for keywords by building up a clause that looks for all of the keywords to be present 
                // in any one of the specified fields.
                modifiedQuery += _keywordsQueryBuilder.AllOfTheseTermsInAnyOfTheseFields(query.Keywords ?? String.Empty, new[] 
                                {
                                    new SearchField() { FieldName = "reference" },
                                    new SearchField() { FieldName = "title", Boost = 2 },
                                    new SearchField() { FieldName = "organisation" },
                                    new SearchField() { FieldName = "location" },
                                    new SearchField() { FieldName = "jobType" },
                                    new SearchField() { FieldName = "contractType" },
                                    new SearchField() { FieldName = "department" },
                                    new SearchField() { FieldName = "fullText", Boost = 0.5 }
                                }, true);
            }

            // For the salary ranges we need a structure that Examine's fluent API can't build, so build the raw Lucene query instead
            if (_salaryQueryBuilder != null)
            {
                modifiedQuery += _salaryQueryBuilder.SalaryIsWithinAnyOfTheseRanges(query.SalaryRanges);
            }

            // Append a requirement that the job must not have closed
            if (query.ClosingDateFrom.HasValue)
            {
                modifiedQuery += " +closingDate:[" + query.ClosingDateFrom.Value.ToString("yyyyMMdd000000000") + " TO 30000000000000000]";
            }

            var luceneQuery = _searcher.CreateSearchCriteria(BooleanOperation.And);

            var sortField = String.Empty;
            switch (query.SortBy)
            {
                case JobSearchQuery.JobsSortOrder.JobTitleAscending:
                case JobSearchQuery.JobsSortOrder.JobTitleDescending:
                    sortField = "titleDisplay";
                    break;
                case JobSearchQuery.JobsSortOrder.LocationAscending:
                case JobSearchQuery.JobsSortOrder.LocationDescending:
                    sortField = "locationDisplay";
                    break;
                case JobSearchQuery.JobsSortOrder.SalaryRangeAscending:
                case JobSearchQuery.JobsSortOrder.SalaryRangeDescending:
                    sortField = "salarySort";
                    break;
                case JobSearchQuery.JobsSortOrder.WorkPatternAscending:
                case JobSearchQuery.JobsSortOrder.WorkPatternDescending:
                    sortField = "workPattern";
                    break;
                case JobSearchQuery.JobsSortOrder.ClosingDateAscending:
                case JobSearchQuery.JobsSortOrder.ClosingDateDescending:
                    sortField = "closingDate";
                    break;
            }

            if (String.IsNullOrWhiteSpace(modifiedQuery))
            {
                modifiedQuery = "__NodeId:[0 TO 999999]";
            }

            try
            {
                switch (query.SortBy)
                {
                    case JobSearchQuery.JobsSortOrder.JobTitleAscending:
                    case JobSearchQuery.JobsSortOrder.LocationAscending:
                    case JobSearchQuery.JobsSortOrder.SalaryRangeAscending:
                    case JobSearchQuery.JobsSortOrder.WorkPatternAscending:
                    case JobSearchQuery.JobsSortOrder.ClosingDateAscending:
                        luceneQuery.RawQuery(modifiedQuery).OrderBy(sortField);
                        break;
                    case JobSearchQuery.JobsSortOrder.JobTitleDescending:
                    case JobSearchQuery.JobsSortOrder.LocationDescending:
                    case JobSearchQuery.JobsSortOrder.SalaryRangeDescending:
                    case JobSearchQuery.JobsSortOrder.WorkPatternDescending:
                    case JobSearchQuery.JobsSortOrder.ClosingDateDescending:
                        luceneQuery.RawQuery(modifiedQuery).OrderByDescending(sortField);
                        break;
                    default:
                        luceneQuery.RawQuery(modifiedQuery);
                        break;
                }

                var results = _searcher.Search(luceneQuery);

                var searchResult = new JobSearchResult()
                {
                    TotalJobs = results.Count()
                };

                IEnumerable<SearchResult> selectedResults;
                if (query.PageSize.HasValue)
                {
                    selectedResults = results.Skip((query.CurrentPage - 1) * query.PageSize.Value).Take(query.PageSize.Value);
                }
                else
                {
                    selectedResults = results;
                }

                searchResult.Jobs = BuildJobsFromExamineResults(selectedResults);
                return Task.FromResult(searchResult);
            }
            catch (ParseException exception)
            {
                exception.Data.Add("Reference", query.JobReference);
                exception.Data.Add("Job types", String.Join(",", query.JobTypes.ToArray()));
                exception.Data.Add("Keywords in title", query.KeywordsInTitle);
                exception.Data.Add("Keywords", query.Keywords);
                exception.Data.Add("Locations", String.Join(",", query.Locations.ToArray()));
                exception.Data.Add("Organisations", String.Join(",", query.Organisations.ToArray()));
                exception.Data.Add("Salary ranges", String.Join(",", query.SalaryRanges.ToArray()));
                exception.Data.Add("Contract types", String.Join(",", query.ContractTypes.ToArray()));
                exception.Data.Add("Work patterns", String.Join(",", query.WorkPatterns.ToArray()));
                exception.Data.Add("Sort", query.SortBy);
                exception.Data.Add("Generated query", modifiedQuery);
                exception.ToExceptionless().Submit();

                var errorForLog = new StringBuilder($"Lucene.net could not parse {modifiedQuery} generated from parameters:").Append(Environment.NewLine)
                    .Append("Reference:").Append(query.JobReference).Append(Environment.NewLine)
                    .Append("Job types:").Append(String.Join(",", query.JobTypes.ToArray())).Append(Environment.NewLine)
                    .Append("Keywords in title:").Append(query.KeywordsInTitle).Append(Environment.NewLine)
                    .Append("Keywords:").Append(query.Keywords).Append(Environment.NewLine)
                    .Append("Locations:").Append(String.Join(",", query.Locations.ToArray())).Append(Environment.NewLine)
                    .Append("Organisations:").Append(String.Join(",", query.Organisations.ToArray())).Append(Environment.NewLine)
                    .Append("Salary ranges:").Append(String.Join(",", query.SalaryRanges.ToArray())).Append(Environment.NewLine)
                    .Append("Contract types:").Append(String.Join(",", query.ContractTypes.ToArray())).Append(Environment.NewLine)
                    .Append("Work patterns:").Append(String.Join(",", query.WorkPatterns.ToArray())).Append(Environment.NewLine)
                    .Append("Sort:").Append(query.SortBy).Append(Environment.NewLine);
                _log.Error(errorForLog.ToString());

                return Task.FromResult(new JobSearchResult());
            }
        }

        private List<Job> BuildJobsFromExamineResults(IEnumerable<SearchResult> results)
        {
            var jobs = new List<Job>();
            foreach (var result in results)
            {
                try
                {
                    var job = new Job()
                    {
                        Id = result.Fields.ContainsKey("id") ? Int32.Parse(result["id"], CultureInfo.InvariantCulture) : 0,
                        Reference = result.Fields.ContainsKey("reference") ? result["reference"] : String.Empty,
                        JobTitle = result.Fields.ContainsKey("titleDisplay") ? result["titleDisplay"] : String.Empty,
                        Organisation = result.Fields.ContainsKey("organisationDisplay") ? result["organisationDisplay"] : String.Empty,
                        Locations = result.Fields.ContainsKey("locationDisplay") ? result["locationDisplay"].Split(',') : new string[0],
                        JobType = result.Fields.ContainsKey("jobTypeDisplay") ? result["jobTypeDisplay"] : String.Empty,
                        ContractType = result.Fields.ContainsKey("contractType") ? result["contractType"] : String.Empty,
                        Department = result.Fields.ContainsKey("departmentDisplay") ? result["departmentDisplay"] : String.Empty,
                        AdvertHtml = new HtmlString(result.Fields.ContainsKey("fullHtml") ? result["fullHtml"] : String.Empty),
                        AdditionalInformationHtml = new HtmlString(result.Fields.ContainsKey("additionalInfo") ? result["additionalInfo"] : String.Empty),
                        EqualOpportunitiesHtml = new HtmlString(result.Fields.ContainsKey("equalOpportunities") ? result["equalOpportunities"] : String.Empty),
                        ApplyUrl = (result.Fields.ContainsKey("applyUrl") && !String.IsNullOrEmpty(result["applyUrl"])) ? new Uri(result["applyUrl"]) : null
                    };

                    if (result.Fields.ContainsKey("workPattern"))
                    {
                        var patterns = result["workPattern"].Split(',');
                        foreach (var pattern in patterns) job.WorkPattern.WorkPatterns.Add(pattern);
                    }

                    job.Salary.SalaryRange = result.Fields.ContainsKey("salaryDisplay") ? result["salaryDisplay"] : String.Empty;
                    job.Salary.SearchRange = result.Fields.ContainsKey("salaryRange") ? result["salaryRange"] : String.Empty;
                    if (result.Fields.ContainsKey("salaryMin") && !String.IsNullOrEmpty(result["salaryMin"]))
                    {
                        int minimumSalary;
                        Int32.TryParse(result["salaryMin"], out minimumSalary);
                        job.Salary.MinimumSalary = minimumSalary;
                    }
                    if (result.Fields.ContainsKey("salaryMax") && !String.IsNullOrEmpty(result["salaryMax"]))
                    {
                        int maximumSalary;
                        Int32.TryParse(result["salaryMax"], out maximumSalary);
                        job.Salary.MaximumSalary = maximumSalary;
                    }
                    if (result.Fields.ContainsKey("hourlyRate") && !String.IsNullOrEmpty(result["hourlyRate"]))
                    {
                        decimal hourlyRate;
                        Decimal.TryParse(result["hourlyRate"], out hourlyRate);
                        job.Salary.HourlyRate = hourlyRate;
                    }

                    if (_urlGenerator != null)
                    {
                        job.Url = _urlGenerator.GenerateUrl(job);
                    }
                    if (result.Fields.ContainsKey("closingDateDisplay")) job.ClosingDate = DateTime.Parse(result["closingDateDisplay"]);
                    if (result.Fields.ContainsKey("datePublished")) job.DatePublished = new LuceneDateTimeParser().Parse(result["datePublished"]);

                    jobs.Add(job);
                }
                catch (Exception ex)
                {
                    ex.ToExceptionless().Submit();
                    return null;
                }
            }

            return jobs;

        }

        /// <summary>
        /// Reads jobs with missing data.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Job>> ReadProblemJobs()
        {
            const string noSalary = " (*:* -salaryMax:[0 TO 99999]) ";
            const string fallbackSalary = " (salaryMax:0009999 salaryMax:0014999 salaryMax:0019999 salaryMax:0024999 salaryMax:0034999 salaryMax:0049999) ";
            const string noWorkPattern = " *:* -workPattern:[* TO \"z*\"] ";

            try
            {
                var results = _searcher.Search(_searcher.CreateSearchCriteria().RawQuery(noSalary + fallbackSalary + noWorkPattern));
                var jobs = BuildJobsFromExamineResults(results);
                return Task.FromResult((IEnumerable<Job>)jobs);
            }
            catch (ParseException exception)
            {
                exception.ToExceptionless().Submit();
                return Task.FromResult(new List<Job>() as IEnumerable<Job>);
            }
        }
    }
}