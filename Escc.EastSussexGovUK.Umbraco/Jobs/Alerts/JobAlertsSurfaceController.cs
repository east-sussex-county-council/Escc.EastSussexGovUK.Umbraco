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
        public ActionResult SubscribeToAlerts(JobAlert alert)
        {
            var query = HttpUtility.ParseQueryString(Request.Url.Query);
            if (ModelState.IsValid)
            {
                var repo = new AzureTableStorageAlertsRepository();
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
    }
}