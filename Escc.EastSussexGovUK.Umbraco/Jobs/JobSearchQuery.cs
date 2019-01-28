using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Humanizer;
using System.Globalization;
using Escc.Dates;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Settings to filter a list of jobs by
    /// </summary>
    /// <remarks>IList properties need to have externally-visible setters to support MVC model binding</remarks>
    public class JobSearchQuery
    {
        /// <summary>
        /// Gets or sets the set of jobs to query
        /// </summary>
        public JobsSet JobsSet { get; set; }

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
        /// Gets the contract types, eg permanent or fixed-term
        /// </summary>
        /// <value>
        /// The contract types.
        /// </value>
        public IList<string> ContractTypes { get; set; } = new List<string>();

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
        /// Gets or sets the pay grades.
        /// </summary>
        public IList<string> PayGrades { get; set; } = new List<string>();

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
        /// Gets or sets the frequency (in days) with which this query should be run.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or sets the current page if results are to be paged
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Gets or sets the size of the page, or <c>null</c> to request all results are returned
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets a unique hash of the filter settings
        /// </summary>
        /// <returns></returns>
        public string ToHash()
        {
            var allContent = new StringBuilder();
            allContent.Append("jobset").Append(JobsSet);
            allContent.Append("keywords").Append(Keywords);
            allContent.Append("keywordsintitle").Append(KeywordsInTitle);
            allContent.Append("location");
            foreach (var value in Locations) allContent.Append(value);
            allContent.Append("organisation");
            foreach (var value in Organisations) allContent.Append(value);
            allContent.Append("salary");
            foreach (var value in SalaryRanges) allContent.Append(value);
            allContent.Append("paygrade");
            foreach (var value in PayGrades) allContent.Append(value);
            allContent.Append("ref").Append(JobReference);
            allContent.Append("types");
            foreach (var value in JobTypes) allContent.Append(value);
            allContent.Append("workpatterns");
            foreach (var value in WorkPatterns) allContent.Append(value);
            allContent.Append("contracttypes");
            foreach (var value in ContractTypes) allContent.Append(value);
            allContent.Append("closingdatefrom").Append(ClosingDateFrom.HasValue ? ClosingDateFrom.ToIso8601DateTime() : String.Empty);
            allContent.Append("sort").Append(SortBy);
            allContent.Append("frequency").Append(Frequency);
            allContent.Append("currentpage").Append(CurrentPage);
            allContent.Append("pagesize").Append(PageSize);

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

        /// <summary>
        /// Gets a plain-English description of the query
        /// </summary>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        /// Gets a plain-English description of the query
        /// </summary>
        /// <param name="startWithACapitalLetter">if set to <c>true</c> the description will always start with a capital letter.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(bool startWithACapitalLetter)
        {
            // Format is [Work pattern] [contract type] [job type] jobs [in location] [paying salary range] [advertised by organisation] [and matching keywords/reference]
            const string defaultDescription = "all jobs";
            var description = new StringBuilder();
            var lowerCaseTransformer = new LowerCaseExceptAcronyms();

            if (WorkPatterns.Count > 0)
            {
                description.Append(ListOfThings(String.Empty, WorkPatterns, lowerCaseTransformer, "and"));
            }

            if (ContractTypes.Count > 0)
            {
                description.Append(ListOfThings(String.Empty, ContractTypes, lowerCaseTransformer, "and"));
            }

            if (JobTypes.Count > 0)
            {
                description.Append(ListOfThings(String.Empty, JobTypes, lowerCaseTransformer, "and"));
            }

            description.Append(" jobs");

            var anythingAfterJobs = false;

            if (Locations.Count > 0)
            {
                description.Append(ListOfThings("in", Locations, null, "or"));
                anythingAfterJobs = true;
            }

            if (SalaryRanges.Count > 0 || PayGrades.Count > 0)
            {
                var allPayLevels = new List<string>();
                allPayLevels.AddRange(SalaryRanges);
                allPayLevels.AddRange(PayGrades);
                description.Append(ListOfThings("paying", allPayLevels, null, "or"));
                anythingAfterJobs = true;
            }

            if (Organisations.Count > 0)
            {
                description.Append(ListOfThings("advertised by", Organisations, null, "or"));
                anythingAfterJobs = true;
            }

            var matching = new List<string>();
            if (!String.IsNullOrEmpty(Keywords)) matching.Add($"'{Keywords}'");
            if (!String.IsNullOrEmpty(JobReference)) matching.Add($"'{JobReference}'");
            description.Append(ListOfThings(anythingAfterJobs ? "and matching" : "matching", matching, null, "or"));

            var result = description.Length > 5 ? description.ToString().TrimStart() : defaultDescription;
            return startWithACapitalLetter ? result.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + result.Substring(1) : result.ToString();
        }

        private string ListOfThings(string conjunctionBeforeList, IList<string> things, IStringTransformer caseTransform, string conjunctionBeforeLastItem)
        {
            if (things.Count == 0) return String.Empty;

            var clause = new StringBuilder(" ");
            if (!String.IsNullOrEmpty(conjunctionBeforeList)) clause.Append(conjunctionBeforeList).Append(" ");

            for (var i = 0; i < things.Count; i++)
            {
                if (i > 0 && i < (things.Count - 1)) clause.Append(", ");
                if (i > 0 && i == things.Count - 1) clause.Append(" ").Append(conjunctionBeforeLastItem).Append(" ");
                if (caseTransform != null)
                {
                    clause.Append(things[i].Transform(caseTransform));
                }
                else
                {
                    clause.Append(things[i]);
                }
            }

            return clause.ToString();
        }

        private class LowerCaseExceptAcronyms : IStringTransformer
        {
            public string Transform(string input)
            {
                const char space = ' ';
                var words = input.Split(space);
                var joined = new StringBuilder();
                foreach (var word in words)
                {
                    joined.Append(space);
                    if (word.ToUpper(CultureInfo.CurrentCulture) == word)
                    {
                        joined.Append(word);
                    }
                    else
                    {
                        joined.Append(word.ToLower(CultureInfo.CurrentCulture));
                    }
                }
                return joined.ToString().TrimStart();
            }
        }
    }
}