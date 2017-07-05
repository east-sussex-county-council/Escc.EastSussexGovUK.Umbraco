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

                query.Add("subscribed", "1");
            }
            return new RedirectToUmbracoPageResult(CurrentPage, query);
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
                    Email = oldAlert.Email
                };
                newAlert.AlertId = encoder.GenerateId(newAlert);

                if (oldAlert.AlertId != newAlert.AlertId)
                {
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