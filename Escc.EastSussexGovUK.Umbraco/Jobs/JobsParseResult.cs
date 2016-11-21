using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Gets the result of parsing jobs from a paged feed of jobs
    /// </summary>
    public class JobsParseResult
    {
        /// <summary>
        /// Gets the jobs.
        /// </summary>
        public IList<Job> Jobs { get; private set; } = new List<Job>();

        /// <summary>
        /// Gets or sets whether this is the last page of jobs in the feed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is last page; otherwise, <c>false</c>.
        /// </value>
        public bool IsLastPage { get; set; }
    }
}