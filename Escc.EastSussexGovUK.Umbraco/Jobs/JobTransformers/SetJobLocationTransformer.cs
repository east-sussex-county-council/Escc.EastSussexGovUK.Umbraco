using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.JobTransformers
{
    /// <summary>
    /// Replaces the locations of a job with those the transformer is configured with
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.JobTransformers.IJobTransformer" />
    public class SetJobLocationTransformer : IJobTransformer
    {
        private readonly IEnumerable<string> _locations;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetJobLocationTransformer"/> class.
        /// </summary>
        /// <param name="locations">The replacement locations.</param>
        /// <exception cref="ArgumentNullException">locations</exception>
        public SetJobLocationTransformer(IEnumerable<string> locations)
        {
            _locations = locations ?? throw new ArgumentNullException(nameof(locations));
        }

        /// <summary>
        /// Transforms the job.
        /// </summary>
        /// <param name="job">The job.</param>
        public void TransformJob(Job job)
        {
            job.Locations = new List<string>(_locations);
        }
    }
}