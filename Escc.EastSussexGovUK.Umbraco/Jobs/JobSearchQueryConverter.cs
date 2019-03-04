using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;
using Escc.Dates;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Converts a <see cref="JobSearchQuery"/> to and from a <see cref="NameValueCollection"/> format
    /// </summary>
    public class JobSearchQueryConverter : IJobSearchQueryConverter
    {
        private readonly bool _translateObsoleteJobTypes;

        /// <summary>
        /// Creates a new instance of <see cref="JobSearchQueryConverter"/>
        /// </summary>
        /// <param name="translateObsoleteJobTypes"><c>true</c> to apply hard-coded translations of old job types; <c>false</c> to query job types exactly as provided</param>
        public JobSearchQueryConverter(bool translateObsoleteJobTypes)
        {
            _translateObsoleteJobTypes = translateObsoleteJobTypes;
        }

        /// <summary>
        /// Creates a <see cref="JobSearchQuery"/> from a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="collection">The query as a collection of parameters.</param>
        /// <returns></returns>
        public JobSearchQuery ToQuery(NameValueCollection collection)
        {
            var query = new JobSearchQuery();

            if (!String.IsNullOrEmpty(collection["keywords"]))
            {
                query.Keywords = collection["keywords"];
            }

            if (!String.IsNullOrEmpty(collection["locations"]))
            {
                AddQueryStringValuesToList(collection["locations"], query.Locations);
            }
            else if (!String.IsNullOrEmpty(collection["location"]))
            {
                // The key was changed to let MVC build a model automatically from form data, 
                // but we need to support the old key as it may be saved in job alert subscriptions
                AddQueryStringValuesToList(collection["location"], query.Locations);
            }

            if (!String.IsNullOrEmpty(collection["jobtypes"]))
            {
                AddQueryStringValuesToList(collection["jobtypes"], query.JobTypes);
            }
            else if (!String.IsNullOrEmpty(collection["type"]))
            {
                // The key was changed to let MVC build a model automatically from form data, 
                // but we need to support the old key as it may be saved in job alert subscriptions
                AddQueryStringValuesToList(collection["type"], query.JobTypes);
            }

            // Translate obsolete job types to their replacements. These are no longer available but may 
            // be saved in old job alert subscriptions.
            if (_translateObsoleteJobTypes)
            {
                TranslateOldJobType("Apprenticeships", new[] { "Apprenticeships and Trainee" }, query.JobTypes);
                TranslateOldJobType("Care and Social Work", new[] { "Bereavement", "Care - Adults", "Safeguarding", "Social Work - Childrens", "Social Work - Adults" }, query.JobTypes);
                TranslateOldJobType("Children’s - Social Work", new[] { "Social Work - Childrens" }, query.JobTypes);
                TranslateOldJobType("Children’s - Social Work Managers", new[] { "Social Work - Childrens" }, query.JobTypes);
                TranslateOldJobType("Education", new[] { "Education and Learning" }, query.JobTypes);
                TranslateOldJobType("ICT", new[] { "Information Technology" }, query.JobTypes);
                TranslateOldJobType("Personnel and HR", new[] { "Human Resources" }, query.JobTypes);
                TranslateOldJobType("Support Staff", new[] { "Support Staff - Education" }, query.JobTypes);
                TranslateOldJobType("Teaching", new[] { "Teaching and Leadership" }, query.JobTypes);
                TranslateOldJobType("Teaching - Leadership", new[] { "Teaching and Leadership" }, query.JobTypes);
                TranslateOldJobType("Transport", new[] { "Transportation and Highways", "Drivers", "Highway Maintenance" }, query.JobTypes);
                TranslateOldJobType("Youth Services", new[] { "Youth and Community Workers" }, query.JobTypes);
                TranslateOldJobType("Youth Work", new[] { "Youth and Community Workers" }, query.JobTypes);
            }

            if (!String.IsNullOrEmpty(collection["department"]))
            {
                AddQueryStringValuesToList(collection["department"], query.Departments);
            }

            if (!String.IsNullOrEmpty(collection["org"]))
            {
                AddQueryStringValuesToList(collection["org"], query.Organisations);
            }

            if (!String.IsNullOrEmpty(collection["salaryranges"]))
            {
                AddQueryStringValuesToList(collection["salaryranges"], query.SalaryRanges);
            }
            else if (!String.IsNullOrEmpty(collection["salary"]))
            {
                // The key was changed to let MVC build a model automatically from form data, 
                // but we need to support the old key as it may be saved in job alert subscriptions
                AddQueryStringValuesToList(collection["salary"], query.SalaryRanges);
            }
            for (var i = 0; i < query.SalaryRanges.Count; i++)
            {
                query.SalaryRanges[i] = Regex.Replace(query.SalaryRanges[i], "^(£)([0-9]+)", FormatSalary_MatchEvaluator);
                query.SalaryRanges[i] = Regex.Replace(query.SalaryRanges[i], "(to £)([0-9]+)", FormatSalary_MatchEvaluator);
            }

            if (!String.IsNullOrEmpty(collection["paygrades"]))
            {
                AddQueryStringValuesToList(collection["paygrades"], query.PayGrades);
            }

            if (!String.IsNullOrEmpty(collection["ref"]))
            {
                query.JobReference = collection["ref"];
            }

            if (!String.IsNullOrEmpty(collection["contracttypes"]))
            {
                AddQueryStringValuesToList(collection["contracttypes"], query.ContractTypes);
            }

            if (!String.IsNullOrEmpty(collection["workpatterns"]))
            {
                AddQueryStringValuesToList(collection["workpatterns"], query.WorkPatterns);
            }

            if (!String.IsNullOrEmpty(collection["closingdatefrom"]))
            {
                if (DateTime.TryParse(collection["closingdatefrom"], out DateTime closingDateFrom))
                {
                    query.ClosingDateFrom = closingDateFrom;
                }
            }

            if (!String.IsNullOrEmpty(collection["sort"]))
            {
                JobSearchQuery.JobsSortOrder sort = JobSearchQuery.JobsSortOrder.None;
                Enum.TryParse(collection["sort"], true, out sort);
                query.SortBy = sort;
            }

            if (!String.IsNullOrEmpty(collection["page"]))
            {
                if (Int32.TryParse(collection["page"], out int page))
                {
                    if (page > 0) query.CurrentPage = page;
                }
            }

            if (!String.IsNullOrEmpty(collection["pagesize"]))
            {
                if (Int32.TryParse(collection["pagesize"], out int pageSize))
                {
                    if (pageSize > 0) query.PageSize = pageSize;
                }
            }

            return query;
        }

        private void TranslateOldJobType(string obsoleteJobType, IEnumerable<string> replacementJobTypes, IList<string> jobTypes)
        {
            if (jobTypes.Contains(obsoleteJobType))
            {
                jobTypes.Remove(obsoleteJobType);
                foreach (var replacement in replacementJobTypes) jobTypes.Add(replacement);
            }
        }

        private string FormatSalary_MatchEvaluator(Match match)
        {
            var salary = Int32.Parse(match.Groups[2].Value);
            return match.Groups[1].Value + salary.ToString("n0");
        }

        private static void AddQueryStringValuesToList(string unvalidatedValue, IList<string> valuesToQuery)
        {
            unvalidatedValue = HttpUtility.UrlDecode(unvalidatedValue);
            var values = Regex.Replace(unvalidatedValue, "[^A-Za-z0-9-,'’ £]", String.Empty).Split(',');
            foreach (var value in values)
            {
                valuesToQuery.Add(value?.Trim());
            }
        }

        /// <summary>
        /// Converts a <see cref="JobSearchQuery"/> to a <see cref="NameValueCollection"/> format
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public NameValueCollection ToCollection(JobSearchQuery query)
        {
            // Use HttpUtility to get a new NameValueCollection(), because this way gets a subclass with a ToString() that serialises as a querystring
            var queryString = HttpUtility.ParseQueryString(String.Empty);
            if (query != null)
            {
                if (!String.IsNullOrEmpty(query.Keywords)) queryString.Add("keywords", query.Keywords);
                if (!String.IsNullOrEmpty(query.JobReference)) queryString.Add("ref", query.JobReference);
                if (query.CurrentPage > 0) queryString.Add("page", query.CurrentPage.ToString(CultureInfo.InvariantCulture));
                if (query.PageSize.HasValue) queryString.Add("pagesize", query.PageSize.Value.ToString(CultureInfo.InvariantCulture));
                if (query.ClosingDateFrom.HasValue) queryString.Add("closingdatefrom", query.ClosingDateFrom.Value.ToIso8601Date());
                if (query.SortBy != JobSearchQuery.JobsSortOrder.None) queryString.Add("sort", query.SortBy.ToString().ToLowerInvariant());

                foreach (var value in query.JobTypes) queryString.Add("jobtypes", value);
                foreach (var value in query.Locations) queryString.Add("locations", value);
                foreach (var value in query.Departments) queryString.Add("department", value);
                foreach (var value in query.Organisations) queryString.Add("org", value);
                foreach (var value in query.SalaryRanges) queryString.Add("salaryranges", value.Replace(",",String.Empty));
                foreach (var value in query.PayGrades) queryString.Add("paygrades", value);
                foreach (var value in query.ContractTypes) queryString.Add("contracttypes", value);
                foreach (var value in query.WorkPatterns) queryString.Add("workpatterns", value);
            }
            return queryString;
        }
    }
}