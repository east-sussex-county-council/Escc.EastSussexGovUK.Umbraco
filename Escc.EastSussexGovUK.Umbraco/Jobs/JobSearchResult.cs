using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// The result of a search for jobs
    /// </summary>
    public class JobSearchResult
    {
        /// <summary>
        /// Gets or sets the jobs.
        /// </summary>
        public List<Job> Jobs { get; set; }

        /// <summary>
        /// Gets or sets the total jobs found, which may be greater than <see cref="Jobs.Count"/> if the query was for a paged result set
        /// </summary>
        public int TotalJobs { get; set; }
    }
}