using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers
{
    /// <summary>
    /// Matches a job in the Joint Community Rehabilitation (JCR) team
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.JobTransformers.IJobMatcher" />
    public class JointCommunityRehabilitationMatcher : IJobMatcher
    {
        /// <summary>
        /// Determines whether the specified job matches the criteria.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns>
        ///   <c>true</c> if the specified job is a match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(Job job)
        {
            if (!String.IsNullOrEmpty(job.JobTitle) && Regex.IsMatch(job.JobTitle, @"\bJCR\b")) return true;
            if (job.AdvertHtml != null && job.AdvertHtml.ToHtmlString().Contains("Joint Community Rehabilitation")) return true;
            return false;
        }
    }
}