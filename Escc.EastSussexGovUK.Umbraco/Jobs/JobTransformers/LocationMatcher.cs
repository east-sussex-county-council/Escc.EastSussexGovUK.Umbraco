using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.JobTransformers
{
    /// <summary>
    /// Matches a job if its locations exactly match those the matcher is configured for
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.JobTransformers.IJobMatcher" />
    public class LocationMatcher : IJobMatcher
    {
        private readonly string _location;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationMatcher"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <exception cref="ArgumentException">The location to match must be specified - location</exception>
        public LocationMatcher(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentException("The location to match must be specified", nameof(location));
            }

            _location = location;
        }

        /// <summary>
        /// Determines whether the specified job matches the criteria.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns>
        ///   <c>true</c> if the specified job is a match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(Job job)
        {
            return job != null && job.Locations.Count == 1 && job.Locations[0] == _location;
        }
    }
}