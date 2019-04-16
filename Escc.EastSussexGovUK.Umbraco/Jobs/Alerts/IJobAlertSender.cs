using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// A way to send out job alerts
    /// </summary>
    interface IJobAlertSender
    {
        /// <summary>
        /// Sends a confirmation that a new alert has been set up.
        /// </summary>
        /// <param name="alert">The alert.</param>
        void SendNewAlertConfirmation(JobAlert alert);

        /// <summary>
        /// Sends alerts which have been grouped by the user to whom they must be sent
        /// </summary>
        /// <param name="groupedAlerts">The grouped alerts.</param>
        /// <param name="repo">The alerts repository where sent alerts can be recorded.</param>
        Task SendGroupedAlerts(IEnumerable<IList<JobAlert>> groupedAlerts, IJobAlertsRepository repo);
    }
}
