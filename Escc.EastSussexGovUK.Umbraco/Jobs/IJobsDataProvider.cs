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
        /// Gets the jobs matching the supplied query
        /// </summary>
        /// <returns></returns>
        Task<List<Job>> ReadJobs(JobSearchQuery query);

        /// <summary>
        /// Reads the locations where jobs can be based
        /// </summary>
        /// <returns></returns>
        Task<IList<JobsLookupValue>> ReadLocations();

        /// <summary>
        /// Reads the job types or categories
        /// </summary>
        /// <returns></returns>
        Task<IList<JobsLookupValue>> ReadJobTypes();

        /// <summary>
        /// Reads the salary ranges that jobs can be categorised as
        /// </summary>
        /// <returns></returns>
        Task<IList<JobsLookupValue>> ReadSalaryRanges();

        /// <summary>
        /// Reads the work patterns, eg full time or part time
        /// </summary>
        /// <returns></returns>
        Task<IList<JobsLookupValue>> ReadWorkPatterns();
    }
}