using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// A query filter for searching for job alerts in an <see cref="IAlertsRepository"/>
    /// </summary>
    public class JobAlertsQuery
    {
        /// <summary>
        /// Gets or sets the email address that matching alerts must be for
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the frequency (in days) that matching alerts must be set to
        /// </summary>
        public int? Frequency { get; set; }

        /// <summary>
        /// Gets the set of jobs and job alerts to search
        /// </summary>
        public JobsSet JobsSet { get; set; }
    }
}