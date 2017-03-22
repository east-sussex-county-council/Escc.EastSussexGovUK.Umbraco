using System;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// A method of generating URLs for jobs
    /// </summary>
    public interface IJobUrlGenerator
    {
        /// <summary>
        /// Generates a URL for a job.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        Uri GenerateUrl(Job job);
    }
}