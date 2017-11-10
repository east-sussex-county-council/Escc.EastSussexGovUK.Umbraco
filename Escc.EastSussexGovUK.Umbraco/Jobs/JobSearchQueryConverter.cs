﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Exceptionless.Extensions;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Converts a <see cref="JobSearchQuery"/> to and from a <see cref="NameValueCollection"/> format
    /// </summary>
    public class JobSearchQueryConverter : IJobSearchQueryConverter
    {
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

            if (!String.IsNullOrEmpty(collection["location"]))
            {
                AddQueryStringValuesToList(collection["location"], query.Locations);
            }

            if (!String.IsNullOrEmpty(collection["type"]))
            {
                AddQueryStringValuesToList(collection["type"], query.JobTypes);
            }

            if (!String.IsNullOrEmpty(collection["org"]))
            {
                AddQueryStringValuesToList(collection["org"], query.Organisations);
            }

            if (!String.IsNullOrEmpty(collection["salary"]))
            {
                AddQueryStringValuesToList(collection["salary"], query.SalaryRanges);
                for (var i = 0; i < query.SalaryRanges.Count; i++)
                {
                    query.SalaryRanges[i] = Regex.Replace(query.SalaryRanges[i], "^(£)([0-9]+)", FormatSalary_MatchEvaluator);
                    query.SalaryRanges[i] = Regex.Replace(query.SalaryRanges[i], "(to £)([0-9]+)", FormatSalary_MatchEvaluator);
                }
            }

            if (!String.IsNullOrEmpty(collection["ref"]))
            {
                query.JobReference = collection["ref"];
            }

            if (!String.IsNullOrEmpty(collection["workpatterns"]))
            {
                AddQueryStringValuesToList(collection["workpatterns"], query.WorkPatterns);
            }

            if (!String.IsNullOrEmpty(collection["sort"]))
            {
                JobSearchQuery.JobsSortOrder sort = JobSearchQuery.JobsSortOrder.None;
                Enum.TryParse(collection["sort"], true, out sort);
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

                foreach (var value in query.JobTypes) queryString.Add("type", value);
                foreach (var value in query.Locations) queryString.Add("location", value);
                foreach (var value in query.Organisations) queryString.Add("org", value);
                foreach (var value in query.SalaryRanges) queryString.Add("salary", value);
                foreach (var value in query.WorkPatterns) queryString.Add("workpatterns", value);
            }
            return queryString;
        }
    }
}