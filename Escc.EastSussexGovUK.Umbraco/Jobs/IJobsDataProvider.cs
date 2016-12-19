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
        /// Reads the HTML for the search fields
        /// </summary>
        /// <returns></returns>
        Task<Stream> ReadSearchFieldsHtml();
        
        /// <summary>
        /// Gets the jobs
        /// </summary>
        /// <returns></returns>
        Task<List<Job>> ReadJobs(JobSearchFilter filter);
    }
}