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
                AddLookupIdsToList(queryString["location"], query.Locations);
            }

            if (!String.IsNullOrEmpty(queryString["type"]))
            {
                AddLookupIdsToList(queryString["type"], query.JobTypes);
            }

            if (!String.IsNullOrEmpty(queryString["org"]))
            {
                AddLookupIdsToList(queryString["org"], query.Organisations);
            }

            if (!String.IsNullOrEmpty(queryString["salary"]))
            {
                AddLookupIdsToList(queryString["salary"], query.SalaryRanges);
            }

            if (!String.IsNullOrEmpty(queryString["ref"]))
            {
                query.JobReference = queryString["ref"];
            }

            if (!String.IsNullOrEmpty(queryString["hours"]))
            {
                AddLookupIdsToList(queryString["hours"], query.WorkPatterns);
            }

            if (!String.IsNullOrEmpty(queryString["sort"]))
            {
                JobSearchQuery.JobsSortOrder sort = JobSearchQuery.JobsSortOrder.None;
                Enum.TryParse(queryString["sort"], true, out sort);
                query.SortBy = sort;
            }

            return query;
        }

        private static void AddLookupIdsToList(string unvalidatedValue, IList<string> valuesToQuery)
        {
            if (Regex.IsMatch(unvalidatedValue, "^[0-9,]+$"))
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