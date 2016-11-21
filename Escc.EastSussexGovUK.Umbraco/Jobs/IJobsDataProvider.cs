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
        /// Gets the jobs
        /// </summary>
        /// <returns></returns>
        Task<List<Job>> ReadJobs();
    }
}