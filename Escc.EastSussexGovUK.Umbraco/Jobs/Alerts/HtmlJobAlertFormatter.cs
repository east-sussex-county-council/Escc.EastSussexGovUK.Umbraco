using Escc.Dates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        private string _newAlertTemplateHtml;
        private string _newJobsTemplateHtml;

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

            ReadTemplateHtml();
        }

        private void ReadTemplateHtml()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var streamReader = new StreamReader(assembly.GetManifestResourceStream("Escc.EastSussexGovUK.Umbraco.Jobs.Alerts.JobAlertConfirmation.html")))
            {
                _newAlertTemplateHtml = streamReader.ReadToEnd();
            }
            using (var streamReader = new StreamReader(assembly.GetManifestResourceStream("Escc.EastSussexGovUK.Umbraco.Jobs.Alerts.JobAlert.html")))
            {
                _newJobsTemplateHtml = streamReader.ReadToEnd();
            }
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
                    emailHtml.Append("<h2 style=\"margin-bottom:0\">").Append(alert.Query).Append("</h2>");
                    emailHtml.Append("<p style=\"margin:.2em 0 1em 0\"><small><a href=\"").Append(_encoder.AddIdToUrl(_alertSettings.ChangeAlertBaseUrl, alert.AlertId)).Append("\">Change or cancel this alert</a></small></p>");
                    emailHtml.Append("<ul style=\"margin-left:0;padding-top:1em\">");
                    foreach (var job in alert.MatchingJobs)
                    {
                        var description = $"{job.WorkPattern} {job.ContractType.ToLower(CultureInfo.CurrentCulture)} {job.Salary.SalaryRange} in {job.Locations}. Closing date {job.ClosingDate.Value.ToBritishDate()}. {job.Organisation} / {job.JobType} vacancy.".TrimStart();
                        description = description.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + description.Substring(1);

                        emailHtml.Append("<li style=\"list-style:none;padding:0\"><p style=\"margin:1em 0 .5em 0\"><a href=\"").Append($"{job.Url}?utm_source=job-alert&utm_medium=job-advert-{HttpUtility.UrlEncode(job.Organisation)}&utm_content={job.Reference}&utm_campaign=General-Recruitment").Append("\">").Append(job.JobTitle).Append("</a></p>")
                            .Append("<p style=\"margin:.5em 0 1em 0;\">").Append(description).Append("</p></li>");
                    }
                    emailHtml.Append("</ul>");
                }
            }

            if (emailHtml.Length == 0) return String.Empty;

            var bodyHtml = TidyUpEmailHtml(_alertSettings.AlertEmailBodyHtml);
            if (String.IsNullOrEmpty(bodyHtml))
            {
                bodyHtml = emailHtml.ToString();
            }
            else
            {
                bodyHtml = bodyHtml.Replace("{jobs}", emailHtml.ToString());
            }

            bodyHtml = _newJobsTemplateHtml.Replace("{body}", bodyHtml);

            return bodyHtml;
        }

        /// <summary>
        /// Formats the confirmation that a new alert has been set up.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public string FormatNewAlertConfirmation(JobAlert alert)
        {
            var alertUrl = _encoder.AddIdToUrl(_alertSettings.ChangeAlertBaseUrl, alert.AlertId).ToString();
            var alertDescription = alert.Query.ToString(false);
            var bodyHtml = TidyUpEmailHtml(_alertSettings.NewAlertEmailBodyHtml)
                                .Replace("{alert-description}", alertDescription)
                                .Replace("{change-alert-url}", alertUrl);

            return _newAlertTemplateHtml.Replace("{body}", bodyHtml);
        }

        private string TidyUpEmailHtml(string html)
        {
            html = html.Replace("href=\"/umbraco/", "href=\""); // because Umbraco admin treats some links as relative to the back office
            return html;
        }
    }
}