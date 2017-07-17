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
        /// Sends alerts which have been grouped by the user to whom they must be sent
        /// </summary>
        /// <param name="groupedAlerts">The grouped alerts.</param>
        void SendGroupedAlerts(IEnumerable<IList<JobAlert>> groupedAlerts);
    }
}
