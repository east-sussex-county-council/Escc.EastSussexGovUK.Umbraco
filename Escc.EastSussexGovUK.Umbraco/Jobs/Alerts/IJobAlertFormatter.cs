using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Format alerts that need to be sent into an alert notification
    /// </summary>
    public interface IJobAlertFormatter
    {
        /// <summary>
        /// Formats a group of alerts for one recipient.
        /// </summary>
        /// <param name="alerts">The alerts.</param>
        /// <returns></returns>
        string FormatAlert(IList<JobAlert> alerts);
    }
}
