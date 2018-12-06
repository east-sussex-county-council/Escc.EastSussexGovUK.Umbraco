using Escc.Dates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Cache jobs in memory for 1 hour
    /// </summary>
    /// <remarks>Use absolute expiration rather than sliding expiration as we don't want old jobs to hang around very long</remarks>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IJobCacheStrategy" />
    public class MemoryJobCacheStrategy : IJobCacheStrategy
    {
        private const string _cacheKeyPrefix = "MemoryJobCacheStrategy-";
        private readonly ObjectCache _cache;
        private readonly bool _enforceUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryJobCacheStrategy"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="enforceUpdate">if set to <c>true</c> read methods will return <c>null</c> to lead application code to update the cache.</param>
        /// <exception cref="ArgumentNullException">cache</exception>
        public MemoryJobCacheStrategy(ObjectCache cache, bool enforceUpdate=false)
        {
            this._cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this._enforceUpdate = enforceUpdate;
        }

        private DateTime CalculateCacheTime()
        {
            // Jobs close at midnight, so don't cache beyond then
            var untilMidnightTonight = DateTime.Today.ToUkDateTime().AddDays(1) - DateTime.Now.ToUkDateTime();
            var isMidnightLessThanAnHourAway = (untilMidnightTonight.TotalSeconds < 3600);
            return isMidnightLessThanAnHourAway ? DateTime.UtcNow.AddSeconds(untilMidnightTonight.TotalSeconds) : DateTime.UtcNow.AddHours(1);
        }

        /// <summary>
        /// Caches full details of a job.
        /// </summary>
        /// <param name="job">The job.</param>
        public void CacheJob(Job job)
        {
            if (job == null || job.Id == 0) return;
            _cache.Set(_cacheKeyPrefix + "Job-" + job.Id, job, CalculateCacheTime());
        }

        /// <summary>
        /// Caches a set of jobs along with the query that returned them
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="jobs">The jobs.</param>
        public void CacheJobs(JobSearchQuery query, JobSearchResult jobs)
        {
            if (query == null || jobs == null) return;
            _cache.Set(_cacheKeyPrefix + "Jobs-" + query.ToHash(), jobs, CalculateCacheTime());
        }

        /// <summary>
        /// Caches a set of lookup values.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        public void CacheLookupValues(string key, IList<JobsLookupValue> values)
        {
            if (String.IsNullOrEmpty(key) || values == null) return;
            _cache.Set(_cacheKeyPrefix + "LookupValues-" + key, values, CalculateCacheTime());
        }

        /// <summary>
        /// Reads the full details of a job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>
        /// The job, or <c>null</c> if the result is not in the cache
        /// </returns>
        public Job ReadJob(string jobId)
        {
            if (String.IsNullOrEmpty(jobId) || _enforceUpdate) return null;
            if (_cache[_cacheKeyPrefix + "Job-" + jobId] != null)
            {
                return _cache[_cacheKeyPrefix + "Job-" + jobId] as Job;
            }
            return null;
        }

        /// <summary>
        /// Reads the jobs matching a specific query
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>
        /// Matching jobs, or <c>null</c> if the result is not in the cache
        /// </returns>
        public JobSearchResult ReadJobs(JobSearchQuery query)
        {
            if (query == null || _enforceUpdate) return null;
            var hash = query.ToHash();
            if (_cache[_cacheKeyPrefix + "Jobs-" + hash] != null)
            {
                return _cache[_cacheKeyPrefix + "Jobs-" + hash] as JobSearchResult;
            }
            return null;
        }

        /// <summary>
        /// Reads a set of jobs lookup values.
        /// </summary>
        /// <param name="key">The key, eg locations.</param>
        /// <returns>
        /// The lookup values, or <c>null</c> if they are not in the cache
        /// </returns>
        public IList<JobsLookupValue> ReadLookupValues(string key)
        {
            if (String.IsNullOrEmpty(key) || _enforceUpdate) return null;
            if (_cache[_cacheKeyPrefix + "LookupValues-" + key] != null)
            {
                return _cache[_cacheKeyPrefix + "LookupValues-" + key] as IList<JobsLookupValue>;
            }
            return null;
        }
    }
}