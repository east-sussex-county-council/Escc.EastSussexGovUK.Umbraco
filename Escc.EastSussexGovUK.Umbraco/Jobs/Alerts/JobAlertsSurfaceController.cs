using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using System.Net.Mail;
using Escc.Services;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    public class JobAlertsSurfaceController : SurfaceController
    {
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateAlert(JobAlert alert)
        {
            var query = HttpUtility.ParseQueryString(Request.Url.Query);
            if (ModelState.IsValid)
            {
                var converter = new JobSearchQueryConverter();
                var encoder = new JobAlertIdEncoder(converter);
                var repo = new AzureTableStorageAlertsRepository(converter);
                alert.Query = converter.ToQuery(query);
                alert.AlertId = encoder.GenerateId(alert);
                repo.SaveAlert(alert);

                var jobAlertsSettings = new JobAlertsSettingsFromUmbraco(Umbraco).GetJobAlertsSettings();
                if (jobAlertsSettings.ContainsKey(alert.JobsSet) && !String.IsNullOrEmpty(jobAlertsSettings[alert.JobsSet].NewAlertEmailSubject))
                {
                    var alertUrl = encoder.AddIdToUrl(jobAlertsSettings[alert.JobsSet].ChangeAlertBaseUrl, alert.AlertId).ToString();
                    var alertDescription = alert.Query.ToString(false);
                    jobAlertsSettings[alert.JobsSet].NewAlertEmailBodyHtml = jobAlertsSettings[alert.JobsSet].NewAlertEmailBodyHtml
                                        .Replace("{alert-description}", alertDescription)
                                        .Replace("/umbraco/{change-alert-url}", alertUrl); // because Umbraco admin treats it as a link relative to the back office

                    SendEmail(alert.Email, jobAlertsSettings[alert.JobsSet].NewAlertEmailSubject, jobAlertsSettings[alert.JobsSet].NewAlertEmailBodyHtml);
                }

                query.Add("subscribed", "1");
            }
            return new RedirectToUmbracoPageResult(CurrentPage, query);
        }

        private static void SendEmail(string emailAddress, string subject, string emailHtml)
        {
            var message = new MailMessage();
            message.To.Add(emailAddress);
            message.Subject = subject;
            message.Body = emailHtml;
            message.IsBodyHtml = true;

            var configuration = new ConfigurationServiceRegistry();
            var cache = new HttpContextCacheStrategy();
            var emailService = ServiceContainer.LoadService<IEmailSender>(configuration, cache);
            emailService.SendAsync(message);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CancelAlert()
        {
            var converter = new JobSearchQueryConverter();
            var encoder = new JobAlertIdEncoder(converter);
            var alertId = encoder.ParseIdFromUrl(new Uri(Request.Url, Request.RawUrl));
            var alertsRepo = new AzureTableStorageAlertsRepository(converter);
            var success = alertsRepo.CancelAlert(alertId);

            return new RedirectResult(Request.RawUrl + "?cancelled=" + (success ? "1" : "0"));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ReplaceAlert(JobSearchQuery searchQuery)
        {
            if (ModelState.IsValid)
            {
                var converter = new JobSearchQueryConverter();
                var encoder = new JobAlertIdEncoder(converter);
                var alertId = encoder.ParseIdFromUrl(new Uri(Request.Url, Request.RawUrl));
                var repo = new AzureTableStorageAlertsRepository(converter);
                var oldAlert = repo.GetAlertById(alertId);

                var newAlert = new JobAlert()
                {
                    Query = searchQuery,
                    Email = oldAlert.Email,
                    Frequency = searchQuery.Frequency,
                    JobsSet = searchQuery.JobsSet                    
                };
                newAlert.AlertId = encoder.GenerateId(newAlert);

                if (oldAlert.AlertId == newAlert.AlertId)
                {
                    // The alert id didn't change but the frequency may have, so update the existing alert
                    repo.SaveAlert(newAlert);
                }
                else
                {
                    // The alert id, and therefore the criteria, changed, so save the new alert and delete the old one
                    repo.SaveAlert(newAlert);
                    repo.CancelAlert(oldAlert.AlertId);
                }

                var urlWithoutQueryString = new Uri(Request.Url, new Uri(Request.Url, Request.RawUrl).AbsolutePath);
                var urlWithoutAlertId = encoder.RemoveIdFromUrl(urlWithoutQueryString);
                var urlWithAlertId = encoder.AddIdToUrl(urlWithoutAlertId, newAlert.AlertId);

                return new RedirectResult(urlWithAlertId + "?updated=1");
            }
            else
            {
                return new RedirectResult(Request.RawUrl);
            }
        }
    }
}