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

            var sortedCopy = new List<string>(WorkPatterns);
            if (sortedCopy.Contains(FULL_TIME) && sortedCopy.Contains(PART_TIME))
            {
                sortedCopy.Remove(FULL_TIME);
                sortedCopy.Remove(PART_TIME);
                sortedCopy.Add("Full or part time");
            }
            sortedCopy.Sort();

            foreach (var pattern in sortedCopy)
            {
                if (text.Length > 0) text.Append(", ");
                text.Append(pattern);
            }

            return text.ToString();
        }
    }
}