using System.Collections.Generic;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Stores and retrieves job alerts data 
    /// </summary>
    public interface IJobAlertsRepository
    {
        /// <summary>
        /// Gets all alerts matching the supplied search query
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        Task<IEnumerable<JobAlert>> GetAlerts(JobAlertsQuery query);

        /// <summary>
        /// Gets a single alert by its identifier.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <returns></returns>
        JobAlert GetAlertById(string alertId);

        Task<IList<int>> GetJobsSentForEmail(JobsSet jobsSet, string emailAddress);

        /// <summary>
        /// Saves a new or updated alert.
        /// </summary>
        /// <param name="alert">The alert.</param>
        Task SaveAlert(JobAlert alert);

        /// <summary>
        /// Cancels an alert. If it's the last alert for an email address, also removes all data for that email address within that <see cref="JobsSet"/>.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <returns></returns>
        Task<bool> CancelAlert(string alertId);

        /// <summary>
        /// Records that an alert has been sent to a given email address.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        Task MarkAlertAsSent(JobsSet jobsSet, int jobId, string emailAddress);
    }
}