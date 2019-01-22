using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// The pattern of hours required by a job
    /// </summary>
    public class WorkPattern 
    {
        /// <summary>
        /// The work patterns configured for a job
        /// </summary>
        /// <remarks>Use a property rather than making <see cref="WorkPattern"/> itself a list to preserve backwards compatibility with serialised jobs</remarks>
        public IList<string> WorkPatterns { get; private set; } = new List<string>();

        /// <summary>
        /// Value representing a full time work pattern.
        /// </summary>
        public const string FULL_TIME = "Full time";

        /// <summary>
        /// Value representing a part time work pattern.
        /// </summary>
        public const string PART_TIME = "Part time";

        /// <summary>
        /// How many hours per week does this work pattern expect?
        /// </summary>
        public decimal? HoursPerWeek { get; set; }

        /// <summary>
        /// Gets a phrase describing the work pattern.
        /// </summary>
        public override string ToString()
        {
            var text = new StringBuilder();
            if (WorkPatterns.Contains(FULL_TIME) && WorkPatterns.Contains(PART_TIME)) text.Append("Full or part time");
            else if (WorkPatterns.Contains(FULL_TIME)) text.Append("Full time");
            else if (WorkPatterns.Contains(PART_TIME)) text.Append("Part time");

            foreach (var pattern in this.WorkPatterns)
            {
                if (pattern == FULL_TIME || pattern == PART_TIME) continue;

                if (text.Length > 0) text.Append(", ");
                text.Append(pattern);
            }

            return text.ToString();
        }
    }
}