using Escc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Sends alerts by email, and records that they have been sent
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.Alerts.IJobAlertSender" />
    public class JobAlertsByEmailSender : IJobAlertSender
    {
        private readonly IJobAlertsRepository _repo;
        private readonly JobAlertSettings _alertSettings;
        private readonly IJobAlertFormatter _formatter;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobAlertsByEmailSender"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="alertSettings">The alert settings.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="emailSender">The email sender.</param>
        /// <exception cref="ArgumentNullException">
        /// repo
        /// or
        /// alertSettings
        /// or
        /// formatter
        /// or
        /// emailSender
        /// </exception>
        public JobAlertsByEmailSender(IJobAlertsRepository repo, JobAlertSettings alertSettings, IJobAlertFormatter formatter, IEmailSender emailSender)
        {
            if (repo == null) throw new ArgumentNullException(nameof(repo));
            _repo = repo;

            if (alertSettings == null) throw new ArgumentNullException(nameof(alertSettings));
            _alertSettings = alertSettings;

            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            _formatter = formatter;

            if (emailSender == null) throw new ArgumentNullException(nameof(emailSender));
            _emailSender = emailSender;
        }

        /// <summary>
        /// Sends alerts which have been grouped by the user to whom they must be sent
        /// </summary>
        /// <param name="groupedAlerts">The grouped alerts.</param>
        public void SendGroupedAlerts(IEnumerable<IList<JobAlert>> groupedAlerts)
        {
            foreach (var alertsForAnEmail in groupedAlerts)
            {
                var email = _formatter.FormatAlert(alertsForAnEmail);

                if (!String.IsNullOrEmpty(email))
                {
                    SendEmail(alertsForAnEmail[0].Email, _alertSettings.AlertEmailSubject, email);
                }

                foreach (var alert in alertsForAnEmail)
                {
                    foreach (var job in alert.MatchingJobs)
                    {
                        _repo.MarkAlertAsSent(alert.JobsSet, job.Id, alert.Email);
                    }
                }
            }
        }

        private void SendEmail(string emailAddress, string subject, string emailHtml)
        {
            var message = new MailMessage();
            message.To.Add(emailAddress);
            message.Subject = subject;
            message.Body = emailHtml;
            message.IsBodyHtml = true;

            _emailSender.SendAsync(message);
        }
    }
}