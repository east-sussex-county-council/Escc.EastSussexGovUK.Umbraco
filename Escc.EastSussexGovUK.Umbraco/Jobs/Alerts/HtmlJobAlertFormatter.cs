using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Format a group of job alerts for one recipient as an HTML fragment
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.Alerts.IJobAlertFormatter" />
    public class HtmlJobAlertFormatter : IJobAlertFormatter
    {
        private readonly JobAlertSettings _alertSettings;
        private readonly JobAlertIdEncoder _encoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlJobAlertFormatter"/> class.
        /// </summary>
        /// <param name="alertSettings">The alert settings.</param>
        /// <param name="encoder">The encoder.</param>
        /// <exception cref="ArgumentNullException">
        /// alertSettings
        /// or
        /// encoder
        /// </exception>
        public HtmlJobAlertFormatter(JobAlertSettings alertSettings, JobAlertIdEncoder encoder)
        {
            if (alertSettings == null) throw new ArgumentNullException(nameof(alertSettings));
            _alertSettings = alertSettings;

            if (encoder == null) throw new ArgumentNullException(nameof(encoder));
            _encoder = encoder;
        }

        /// <summary>
        /// Formats a group of alerts for one recipient.
        /// </summary>
        /// <param name="alerts">The alerts.</param>
        /// <returns></returns>
        public string FormatAlert(IList<JobAlert> alerts)
        {
            var emailHtml = new StringBuilder();

            foreach (var alert in alerts)
            {
                if (alert.MatchingJobs.Count > 0)
                {
                    emailHtml.Append("<h2>").Append(alert.Query).Append("</h2><ul>");
                    foreach (var job in alert.MatchingJobs)
                    {
                        emailHtml.Append("<li><a href=\"").Append(job.Url).Append("\">").Append(job.JobTitle).Append("</a></li>");
                    }
                    emailHtml.Append("</ul>");

                    emailHtml.Append("<p><a href=\"").Append(_encoder.AddIdToUrl(_alertSettings.ChangeAlertBaseUrl, alert.AlertId)).Append("\">Change or cancel alert</a></p>");
                }
            }

            if (emailHtml.Length == 0) return String.Empty;

            var bodyHtml = _alertSettings.AlertEmailBodyHtml;
            if (String.IsNullOrEmpty(bodyHtml))
            {
                return emailHtml.ToString();
            }
            else
            {
                bodyHtml = bodyHtml.Replace("{jobs}", emailHtml.ToString());
                return bodyHtml;
            }
        }
    }
}