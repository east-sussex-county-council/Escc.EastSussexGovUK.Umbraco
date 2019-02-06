using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Escc.Services;
using System.Configuration;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs.Alerts
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
                var converter = new JobSearchQueryConverter(ConfigurationManager.AppSettings["TranslateObsoleteJobTypes"]?.ToUpperInvariant() == "TRUE");
                var encoder = new JobAlertIdEncoder(converter);
                var alertsRepo = new AzureTableStorageAlertsRepository(converter, ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);
                alert.Query = converter.ToQuery(query);
                alert.AlertId = encoder.GenerateId(alert);
                alertsRepo.SaveAlert(alert);

                var jobAlertsSettings = new JobAlertsSettingsFromUmbraco(Umbraco).GetJobAlertsSettings(alert.JobsSet);
                if (jobAlertsSettings != null && !String.IsNullOrEmpty(jobAlertsSettings.NewAlertEmailSubject))
                {
                    var emailService = ServiceContainer.LoadService<IEmailSender>(new ConfigurationServiceRegistry(), new HttpContextCacheStrategy());
                    var sender = new JobAlertsByEmailSender(jobAlertsSettings, new HtmlJobAlertFormatter(jobAlertsSettings, encoder), emailService);

                    sender.SendNewAlertConfirmation(alert);
                }

                query.Add("subscribed", "1");
            }
            return new RedirectToUmbracoPageResult(CurrentPage, query);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CancelAlert()
        {
            var converter = new JobSearchQueryConverter(ConfigurationManager.AppSettings["TranslateObsoleteJobTypes"]?.ToUpperInvariant() == "TRUE");
            var encoder = new JobAlertIdEncoder(converter);
            var absoluteUrl = new Uri(Request.Url, Request.RawUrl);
            var alertId = encoder.ParseIdFromUrl(absoluteUrl);
            var alertsRepo = new AzureTableStorageAlertsRepository(converter, ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);
            var success = await alertsRepo.CancelAlert(alertId);

            return new RedirectResult(absoluteUrl.AbsolutePath + "?cancelled=" + (success ? "1" : "0"));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> ReplaceAlert(JobSearchQuery searchQuery)
        {
            if (ModelState.IsValid)
            {
                var converter = new JobSearchQueryConverter(ConfigurationManager.AppSettings["TranslateObsoleteJobTypes"]?.ToUpperInvariant() == "TRUE");
                var encoder = new JobAlertIdEncoder(converter);
                var alertId = encoder.ParseIdFromUrl(new Uri(Request.Url, Request.RawUrl));
                var repo = new AzureTableStorageAlertsRepository(converter, ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);
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
                    await repo.CancelAlert(oldAlert.AlertId);
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