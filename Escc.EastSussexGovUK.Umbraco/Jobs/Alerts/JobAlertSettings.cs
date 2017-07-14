using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Settings to apply to all job alerts for a <see cref="JobsSet"/>
    /// </summary>
    public class JobAlertSettings
    {
        /// <summary>
        /// Gets or sets the base URL for job adverts.
        /// </summary>
        public Uri JobAdvertBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the base URL for the page used to change or cancel an alert.
        /// </summary>
        public Uri ChangeAlertBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the subject for the email confirming a new alert has been created.
        /// </summary>
        public string NewAlertEmailSubject { get; set; }

        /// <summary>
        /// Gets or sets the body HTML for the email confirming a new alert has been created.
        /// </summary>
        public string NewAlertEmailBodyHtml { get; set; }

        /// <summary>
        /// Gets or sets the subject for the alert email.
        /// </summary>
        public string AlertEmailSubject { get; set; }

        /// <summary>
        /// Gets or sets the body HTML for the alert email.
        /// </summary>
        public string AlertEmailBodyHtml { get; set; }
    }
}