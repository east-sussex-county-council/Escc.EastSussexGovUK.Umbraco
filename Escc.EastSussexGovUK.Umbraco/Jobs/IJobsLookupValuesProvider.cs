using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// A way to get some lookup values to use when searching for jobs
    /// </summary>
    public interface IJobsLookupValuesProvider
    {
        /// <summary>
        /// Reads the locations where jobs can be based
        /// </summary>
        /// <returns></returns>
        Task<IList<JobsLookupValue>> ReadLocations();

        /// <summary>
        /// Reads the job types or categories, eg social care or education
        /// </summary>
        /// <returns></returns>
        Task<IList<JobsLookupValue>> ReadJobTypes();

        /// <summary>
        /// Reads the organisations advertising jobs
        /// </summary>
        /// <returns></returns>
        Task<IList<JobsLookupValue>> ReadOrganisations();

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

        /// <summary>
        /// Reads the contract types, eg fixed term or permanent
        /// </summary>
        /// <returns></returns>
        Task<IList<JobsLookupValue>> ReadContractTypes();
     }
}