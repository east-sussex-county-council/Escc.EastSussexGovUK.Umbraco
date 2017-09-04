using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Exceptionless.Extensions;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Creates a <see cref="JobSearchQuery"/>
    /// </summary>
    public class JobSearchQueryFactory
    {
        /// <summary>
        /// Creates a <see cref="JobSearchQuery"/> from query string parameters.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        public JobSearchQuery CreateFromQueryString(NameValueCollection queryString)
        {
            var query = new JobSearchQuery();

            if (!String.IsNullOrEmpty(queryString["keywords"]))
            {
                query.Keywords = queryString["keywords"];
            }

            if (!String.IsNullOrEmpty(queryString["location"]))
            {
                AddQueryStringValuesToList(queryString["location"], query.Locations);
            }

            if (!String.IsNullOrEmpty(queryString["type"]))
            {
                AddQueryStringValuesToList(queryString["type"], query.JobTypes);
            }

            if (!String.IsNullOrEmpty(queryString["org"]))
            {
                AddQueryStringValuesToList(queryString["org"], query.Organisations);
            }

            if (!String.IsNullOrEmpty(queryString["salary"]))
            {
                AddQueryStringValuesToList(queryString["salary"], query.SalaryRanges);
                for (var i=0;i < query.SalaryRanges.Count; i++)
                {
                    query.SalaryRanges[i] = Regex.Replace(query.SalaryRanges[i], "^(£)([0-9]+)", FormatSalary_MatchEvaluator);
                    query.SalaryRanges[i] = Regex.Replace(query.SalaryRanges[i], "(to £)([0-9]+)", FormatSalary_MatchEvaluator);
                }
            }

            if (!String.IsNullOrEmpty(queryString["ref"]))
            {
                query.JobReference = queryString["ref"];
            }

            if (!String.IsNullOrEmpty(queryString["hours"]))
            {
                AddQueryStringValuesToList(queryString["hours"], query.WorkPatterns);
            }

            if (!String.IsNullOrEmpty(queryString["sort"]))
            {
                JobSearchQuery.JobsSortOrder sort = JobSearchQuery.JobsSortOrder.None;
                Enum.TryParse(queryString["sort"], true, out sort);
                query.SortBy = sort;
            }

            return query;
        }

        private string FormatSalary_MatchEvaluator(Match match)
        {
            var salary = Int32.Parse(match.Groups[2].Value);
            return match.Groups[1].Value + salary.ToString("n0");
        }

        private static void AddQueryStringValuesToList(string unvalidatedValue, IList<string> valuesToQuery)
        {
            unvalidatedValue = HttpUtility.UrlDecode(unvalidatedValue);
            var values = Regex.Replace(unvalidatedValue, "[^A-Za-z0-9-,'’ £]", String.Empty).SplitAndTrim(",");
            foreach (var value in values)
            {
                valuesToQuery.Add(value);
            }
        }
    }
}