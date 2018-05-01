using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.JobTransformers
{
    /// <summary>
    /// A test for whether a job matches a certain set of criteria
    /// </summary>
    public interface IJobMatcher
    {
        /// <summary>
        /// Determines whether the specified job matches the criteria.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns>
        ///   <c>true</c> if the specified job is a match; otherwise, <c>false</c>.
        /// </returns>
        bool IsMatch(Job job);
    }
}
