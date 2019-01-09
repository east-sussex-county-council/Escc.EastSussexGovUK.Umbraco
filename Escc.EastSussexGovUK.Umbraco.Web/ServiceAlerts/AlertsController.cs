using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Escc.Dates;
using Escc.Net;
using Escc.Umbraco.PropertyTypes;
using Escc.Web;
using Exceptionless;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.ServiceClosures;

namespace Escc.EastSussexGovUK.Umbraco.Web.ServiceAlerts
{
    public class AlertsController : RenderMvcController
    {
        //
        // GET: /Alerts/

        public override ActionResult Index(RenderModel model)
        {
            var alerts = new List<AlertViewModel>();

            AddAlertsFromUmbraco(model, alerts, new UrlListReader());
            AddSchoolClosureAlerts(alerts);

            // Cache for speed, but not for long because content is time-sensitive
            new HttpCacheHeaders().CacheUntil(Response.Cache, DateTime.Now.ToUkDateTime().AddMinutes(5));

            return CurrentTemplate(alerts);
        }

        /// <summary>
        /// Temporary method to get schools closures alert as HTTP request from old hosting. To be rewritten once school closures available locally.
        /// </summary>
        /// <param name="alerts"></param>
        private void AddSchoolClosureAlerts(List<AlertViewModel> alerts)
        {
            const string cacheKey = "Escc.EastSussexGovUK.Umbraco.SchoolClosures";
            var alertHtml = String.Empty;
            if (HttpContext.Cache[cacheKey] != null && Request.QueryString["ForceCacheRefresh"] != "true")
            {
                alertHtml = HttpContext.Cache[cacheKey] as string;
                AddSchoolClosureAlert(alerts, alertHtml);
                return;
            }

            if (ConfigurationManager.ConnectionStrings["Escc.ServiceClosures.AzureStorage"] == null || String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["Escc.ServiceClosures.AzureStorage"].ConnectionString))
            {
                new ConfigurationErrorsException("The Escc.ServiceClosures.AzureStorage connection string is missing from web.config").ToExceptionless().Submit();
                return;
            }

            var closureDataSource = new AzureBlobStorageDataSource(ConfigurationManager.ConnectionStrings["Escc.ServiceClosures.AzureStorage"].ConnectionString, "service-closures");
            var closureData = closureDataSource.ReadClosureDataAsync(new ServiceType("school")).Result;
            if (closureData != null && (TooLateForToday() ? closureData.EmergencyClosureExists(DateTime.Today.AddDays(1)) : closureData.EmergencyClosureExists(DateTime.Today)))
            {
                alertHtml = "<p><a href=\"https://www.eastsussex.gov.uk/educationandlearning/schools/schoolclosures/\">Emergency school closures</a> &#8211; check if your school is affected, and subscribe to alerts.</p>";

                AddSchoolClosureAlert(alerts, alertHtml);
            }

            // Cache the HTML returned, even if it's String.Empty, so that we don't make too many web requests
            HttpContext.Cache.Insert(cacheKey, alertHtml, null, DateTime.Now.AddMinutes(5),
                Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
        }

        /// <summary>
        /// Gets whether to check for emergency closures today or tomorrow, depending on time of day
        /// </summary>
        /// <returns></returns>
        private bool TooLateForToday()
        {
            return (DateTime.Now.ToUkDateTime() > DateTime.Today.Date.AddHours(15).AddMinutes(30)); // change display after 3.30pm
        }

        private static void AddSchoolClosureAlert(List<AlertViewModel> alerts, string alertHtml)
        {
            if (!String.IsNullOrWhiteSpace(alertHtml))
            {
                var alert = new AlertViewModel()
                 {
                     Alert = new HtmlString(alertHtml),
                     Append = true,
                     Cascade = true
                 };
                alert.TargetUrls.Add(new Uri("/", UriKind.Relative));
                alerts.Add(alert);

            }
        }

        /// <summary>
        /// Get alerts from Umbraco pages which are children of this one, which should all be using the Alert document type
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="alerts">The alerts.</param>
        /// <param name="urlListReader">The URL list reader.</param>
        private static void AddAlertsFromUmbraco(RenderModel model, List<AlertViewModel> alerts, IUrlListReader urlListReader)
        {
            foreach (var alertPage in model.Content.Children)
            {
                var alertModel = new AlertViewModel()
                {
                    Alert = new HtmlString(alertPage.GetPropertyValue<string>("alert_Content")),
                    Append = alertPage.GetPropertyValue<bool>("append_Content"),
                    Cascade = alertPage.GetPropertyValue<bool>("cascade_Content")
                };

                ((List<Uri>)alertModel.TargetUrls).AddRange(urlListReader.ReadUrls(alertPage, "whereToDisplayIt_Content", "whereElseToDisplayIt_Content"));

                if (alertModel.TargetUrls.Count > 0)
                {
                    alerts.Add(alertModel);
                }
            }
        }
    }
}
