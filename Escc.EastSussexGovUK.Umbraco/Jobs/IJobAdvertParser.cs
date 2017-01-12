using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public interface IJobAdvertParser
    {
        /// <summary>
        /// Parses a job.
        /// </summary>
        /// <param name="sourceData">The source data for the job.</param>
        /// <returns></returns>
        Job ParseJob(string sourceData);
    }
}