using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// A way to get some jobs
    /// </summary>
    public interface IJobsDataProvider
    {
        /// <summary>
        /// Gets the job matching the supplied id
        /// </summary>
        /// <returns></returns>
        Task<Job> ReadJob(string jobId);

        /// <summary>
        /// Gets the jobs matching the supplied query
        /// </summary>
        /// <returns></returns>
        Task<List<Job>> ReadJobs(JobSearchQuery query);
    }
}