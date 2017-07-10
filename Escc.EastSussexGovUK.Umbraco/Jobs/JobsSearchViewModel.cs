using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// View model for the search page of the jobs section
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobsSearchViewModel : BaseJobsViewModel
    {
        /// <summary>
        /// Gets or sets the set of jobs to search.
        /// </summary>
        public JobsSet JobsSet { get; set; }
        
        /// <summary>
        /// Gets or sets the search query
        /// </summary>
        public JobSearchQuery Query { get; set; }

        /// <summary>
        /// Gets the locations where jobs can be based
        /// </summary>
        public IList<JobsLookupValue> Locations { get; } = new List<JobsLookupValue>();

        /// <summary>
        /// Gets the job types.
        /// </summary>
        public IList<JobsLookupValue> JobTypes { get; } = new List<JobsLookupValue>();

        /// <summary>
        /// Gets the salary ranges.
        /// </summary>
        public IList<JobsLookupValue> SalaryRanges { get; } = new List<JobsLookupValue>();

        /// <summary>
        /// Gets the work patterns.
        /// </summary>
        /// <value>
        /// The work patterns.
        /// </value>
        public IList<JobsLookupValue> WorkPatterns { get; } = new List<JobsLookupValue>();
    }
}