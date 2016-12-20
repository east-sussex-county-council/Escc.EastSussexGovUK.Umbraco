using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
                query.Keywords = queryString["location"];
            }

            if (!String.IsNullOrEmpty(queryString["type"]))
            {
                query.JobTypes.Add(queryString["type"]);
            }

            if (!String.IsNullOrEmpty(queryString["org"]))
            {
                query.Organisations.Add(queryString["org"]);
            }

            if (!String.IsNullOrEmpty(queryString["salary"]))
            {
                query.SalaryRanges.Add(queryString["salary"]);
            }

            if (!String.IsNullOrEmpty(queryString["ref"]))
            {
                query.JobReference = queryString["ref"];
            }

            if (!String.IsNullOrEmpty(queryString["hours"]))
            {
                var values = queryString["hours"].SplitAndTrim(",");
                foreach (var value in values)
                {
                    query.WorkPatterns.Add(value);
                }
            }

            if (!String.IsNullOrEmpty(queryString["sort"]))
            {
                JobSearchQuery.JobsSortOrder sort = JobSearchQuery.JobsSortOrder.None;
                Enum.TryParse(queryString["sort"], true, out sort);
                query.SortBy = sort;
            }

            return query;
        }
    }
}