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

namespace Escc.EastSussexGovUK.Umbraco.ServiceAlerts
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
            const string cacheKey = "Escc.Alerts.Website.SchoolClosures";
            if (HttpContext.Cache[cacheKey] != null && Request.QueryString["ForceCacheRefresh"] != "true")
            {
                var alertHtml = HttpContext.Cache[cacheKey] as string;
                AddSchoolClosureAlert(alerts, alertHtml);
                return;
            }

            using (var client = new WebClient())
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.Proxy = new ConfigurationProxyProvider().CreateProxy(); // for www.eastsussex.gov.uk
                    if (client.Proxy != null) client.Credentials = client.Proxy.Credentials; // for webcontent
                    var alertHtml = client.DownloadString(new Uri(ConfigurationManager.AppSettings["SchoolClosureAlertsTemporaryApi"], UriKind.RelativeOrAbsolute));
                    AddSchoolClosureAlert(alerts, alertHtml);

                    // Cache the HTML returned, even if it's String.Empty, so that we don't make too many web requests
                    HttpContext.Cache.Insert(cacheKey, alertHtml, null, DateTime.Now.AddMinutes(5),
                        Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
                }
                catch (ArgumentNullException e)
                {
                    e.Data.Add("Missing appSetting", "SchoolClosureAlertsTemporaryApi");
                    e.ToExceptionless().Submit();
                }
                catch (WebException e)
                {
                    e.ToExceptionless().Submit();
                }
            }
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
