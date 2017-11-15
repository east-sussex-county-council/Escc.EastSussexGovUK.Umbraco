using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// A method of caching jobs data returned from a data source
    /// </summary>
    public interface IJobCacheStrategy
    {
        /// <summary>
        /// Reads the jobs matching a specific query
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Matching jobs, or <c>null</c> if the result is not in the cache</returns>
        JobSearchResult ReadJobs(JobSearchQuery query);

        /// <summary>
        /// Caches a set of jobs along with the query that returned them
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="jobs">The jobs.</param>
        void CacheJobs(JobSearchQuery query, JobSearchResult jobs);

        /// <summary>
        /// Reads the full details of a job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>The job, or <c>null</c> if the result is not in the cache</returns>
        Job ReadJob(string jobId);

        /// <summary>
        /// Caches full details of a job.
        /// </summary>
        /// <param name="job">The job.</param>
        void CacheJob(Job job);

        /// <summary>
        /// Reads a set of jobs lookup values.
        /// </summary>
        /// <param name="key">The key, eg locations.</param>
        /// <returns>The lookup values, or <c>null</c> if they are not in the cache</returns>
        IList<JobsLookupValue> ReadLookupValues(string key);

        /// <summary>
        /// Caches a set of lookup values.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        void CacheLookupValues(string key, IList<JobsLookupValue> values);
    }
}
