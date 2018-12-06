using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Get the settings for job alerts for a specific <see cref="JobsSet"/>, each of which could have its own settings
    /// </summary>
    public interface IJobAlertSettingsProvider
    {
        /// <summary>
        /// Gets the job alert settings for a <see cref="JobsSet"/>
        /// </summary>
        /// <returns></returns>
        JobAlertSettings GetJobAlertsSettings(JobsSet jobsSet);
    }
}
