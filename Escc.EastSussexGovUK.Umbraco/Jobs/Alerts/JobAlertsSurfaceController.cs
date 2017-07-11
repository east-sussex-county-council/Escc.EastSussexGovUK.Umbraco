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
                var repo = new AzureTableStorageAlertsRepository();
                alert.AlertId = new JobAlertIdEncoder().GenerateId(alert);
                repo.SaveAlert(alert);

                SendEmail(alert.Email, "<h2>Your email alert has been created</h2><p>" + alert.Criteria + "</p>");

                query.Add("subscribed", "1");
            }
            return new RedirectToUmbracoPageResult(CurrentPage, query);
        }

        private static void SendEmail(string emailAddress, string emailHtml)
        {
            var message = new MailMessage();
            message.To.Add(emailAddress);
            message.Subject = "Your email alert has been created";
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
            var encoder = new JobAlertIdEncoder();
            var alertId = encoder.ParseIdFromUrl(new Uri(Request.Url, Request.RawUrl));
            var alertsRepo = new AzureTableStorageAlertsRepository();
            var success = alertsRepo.CancelAlert(alertId);

            return new RedirectResult(Request.RawUrl + "?cancelled=" + (success ? "1" : "0"));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ReplaceAlert(JobSearchQuery searchQuery)
        {
            if (ModelState.IsValid)
            {
                var encoder = new JobAlertIdEncoder();
                var alertId = encoder.ParseIdFromUrl(new Uri(Request.Url, Request.RawUrl));
                var repo = new AzureTableStorageAlertsRepository();
                var oldAlert = repo.GetAlertById(alertId);

                var newAlert = new JobAlert()
                {
                    Criteria = new JobSearchQueryConverter().ToCollection(searchQuery).ToString(),
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