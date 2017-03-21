using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Settings to filter a list of jobs by
    /// </summary>
    /// <remarks>IList properties need to have externally-visible setters to support MVC model binding</remarks>
    public class JobSearchQuery
    {
        /// <summary>
        /// Gets or sets the keywords to search significant fields by, using an AND operator between words
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the keywords to search the job title by, using an OR operator between words
        /// </summary>
        public string KeywordsInTitle { get; set; }

        /// <summary>
        /// Gets or sets the location where the job is based
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public IList<string> Locations { get; set; } = new List<string>();

        /// <summary>
        /// Gets the job types or categories to limit results to.
        /// </summary>
        /// <value>
        /// The job types.
        /// </value>
        public IList<string> JobTypes { get; set; } = new List<string>();

        /// <summary>
        /// Gets the working hours, eg full-time or part-time
        /// </summary>
        /// <value>
        /// The working hours.
        /// </value>
        public IList<string> WorkPatterns { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the organisation advertising the job
        /// </summary>
        /// <value>
        /// The organisation.
        /// </value>
        public IList<string> Organisations { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the salary band.
        /// </summary>
        /// <value>
        /// The salary band.
        /// </value>
        public IList<string> SalaryRanges { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the job reference.
        /// </summary>
        /// <value>
        /// The job reference.
        /// </value>
        public string JobReference { get; set; }

        /// <summary>
        /// Gets or sets a date that jobs must close on or after
        /// </summary>
        public DateTime? ClosingDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the field to sort results by
        /// </summary>
        /// <value>
        /// The sort by.
        /// </value>
        public JobsSortOrder SortBy { get; set; } = JobsSortOrder.None;

        /// <summary>
        /// The possible sort orders supported by the jobs service
        /// </summary>
        public enum JobsSortOrder
        {
            None,
            JobTitleAscending,
            JobTitleDescending,
            OrganisationAscending,
            OrganisationDescending,
            LocationAscending,
            LocationDescending,
            SalaryRangeAscending,
            SalaryRangeDescending,
            ClosingDateAscending,
            ClosingDateDescending,
            WorkPatternAscending,
            WorkPatternDescending
        }

        /// <summary>
        /// Gets a unique hash of the filter settings
        /// </summary>
        /// <returns></returns>
        public string ToHash()
        {
            var allContent = new StringBuilder();
            allContent.Append("keywords").Append(Keywords);
            allContent.Append("location");
            foreach (var value in Locations) allContent.Append(value);
            allContent.Append("organisation");
            foreach (var value in Organisations) allContent.Append(value);
            allContent.Append("salary");
            foreach (var value in SalaryRanges) allContent.Append(value);
            allContent.Append("ref").Append(JobReference);
            allContent.Append("types");
            foreach (var value in JobTypes) allContent.Append(value);
            allContent.Append("workpatterns");
            foreach (var value in WorkPatterns) allContent.Append(value);
            allContent.Append("sort").Append(SortBy);

            HashAlgorithm algorithm = SHA1.Create();
            var bytes = Encoding.ASCII.GetBytes(allContent.ToString());
            var hash = algorithm.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}