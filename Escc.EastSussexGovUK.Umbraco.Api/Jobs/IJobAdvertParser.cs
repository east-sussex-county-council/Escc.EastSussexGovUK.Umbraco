using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
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