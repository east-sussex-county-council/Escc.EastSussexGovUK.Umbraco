using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// The pattern of hours required by a job
    /// </summary>
    public class WorkPattern
    {
        /// <summary>
        /// Gets or sets whether this job is full time.
        /// </summary>
        public bool IsFullTime { get; set; }

        /// <summary>
        /// Gets or sets whether this job is part time.
        /// </summary>
        public bool IsPartTime { get; set; }

        /// <summary>
        /// Gets a phrase describing the work pattern.
        /// </summary>
        public override string ToString()
        {
            if (IsFullTime && IsPartTime) return "Full or part time";
            if (IsFullTime) return "Full time";
            if (IsPartTime) return "Part time";
            return String.Empty;
        }
    }
}