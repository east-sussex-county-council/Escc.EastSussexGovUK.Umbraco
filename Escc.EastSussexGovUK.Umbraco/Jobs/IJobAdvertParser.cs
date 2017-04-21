using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// A way to parse job data from a string of information
    /// </summary>
    public interface IJobAdvertParser
    {
        /// <summary>
        /// Parses a job.
        /// </summary>
        /// <param name="sourceData">The source data for the job.</param>
        /// <param name="jobId">The job identifier.</param>
        /// <returns></returns>
        Job ParseJob(string sourceData, string jobId);
    }
}