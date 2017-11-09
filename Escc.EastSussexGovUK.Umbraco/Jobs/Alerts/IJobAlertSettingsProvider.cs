using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Get the settings for job alerts, indexed by <see cref="JobsSet"/> which could each have their own settings
    /// </summary>
    public interface IJobAlertSettingsProvider
    {
        /// <summary>
        /// Gets the job alert settings for each <see cref="JobsSet"/>
        /// </summary>
        /// <returns></returns>
        Dictionary<JobsSet,JobAlertSettings> GetJobAlertsSettings();
    }
}
