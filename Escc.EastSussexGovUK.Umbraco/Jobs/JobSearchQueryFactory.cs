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
                AddQueryStringValuesToList(queryString["location"], query.Locations, true);
            }

            if (!String.IsNullOrEmpty(queryString["type"]))
            {
                AddQueryStringValuesToList(queryString["type"], query.JobTypes, true);
            }

            if (!String.IsNullOrEmpty(queryString["org"]))
            {
                AddQueryStringValuesToList(queryString["org"], query.Organisations, true);
            }

            if (!String.IsNullOrEmpty(queryString["salary"]))
            {
                AddQueryStringValuesToList(queryString["salary"], query.SalaryRanges, false);
            }

            if (!String.IsNullOrEmpty(queryString["ref"]))
            {
                query.JobReference = queryString["ref"];
            }

            if (!String.IsNullOrEmpty(queryString["hours"]))
            {
                AddQueryStringValuesToList(queryString["hours"], query.WorkPatterns, true);
            }

            if (!String.IsNullOrEmpty(queryString["sort"]))
            {
                JobSearchQuery.JobsSortOrder sort = JobSearchQuery.JobsSortOrder.None;
                Enum.TryParse(queryString["sort"], true, out sort);
                query.SortBy = sort;
            }

            return query;
        }

        private static void AddQueryStringValuesToList(string unvalidatedValue, IList<string> valuesToQuery, bool mustBeNumeric)
        {
            if (!mustBeNumeric || Regex.IsMatch(unvalidatedValue, "^[0-9,]+$"))
            {
                var values = unvalidatedValue.SplitAndTrim(",");
                foreach (var value in values)
                {
                    valuesToQuery.Add(value);
                }
            }
        }
    }
}