using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Models;
using EsccWebTeam.Data.Web;
using EsccWebTeam.Data.Xml;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using umbraco.cms.helpers;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    public class AlertsController : RenderMvcController
    {
        //
        // GET: /Alerts/

        public override ActionResult Index(RenderModel model)
        {
            var alerts = new List<AlertViewModel>();

            AddAlertsFromUmbraco(model, alerts);
            AddSchoolClosureAlerts(alerts);

            // Cache for speed, but not for long because content is time-sensitive
            Http.CacheFor(0, 5);

            return CurrentTemplate(alerts);
        }

        /// <summary>
        /// Temporary method to get schools closures alert as HTTP request from old hosting. To be rewritten once school closures available locally.
        /// </summary>
        /// <param name="alerts"></param>
        private void AddSchoolClosureAlerts(List<AlertViewModel> alerts)
        {
            var client = new WebClient();
            try
            {
                client.Proxy = new EsccProxy(); // for www.eastsussex.gov.uk
                if (client.Proxy != null) client.Credentials = client.Proxy.Credentials; // for webcontent
                var alertHtml = client.DownloadString(new Uri(ConfigurationManager.AppSettings["SchoolClosureAlertsTemporaryApi"]));
                if (!String.IsNullOrWhiteSpace(alertHtml))
                {
                    alerts.Add(new AlertViewModel()
                    {
                        Alert = new HtmlString(alertHtml),
                        TargetUrls = new List<Uri>() { new Uri("/", UriKind.Relative) },
                        Append = true,
                        Cascade = true
                    });
                }
            }
            catch (WebException ex)
            {
                ExceptionManager.Publish(ex);
            }
        }

        private static void AddAlertsFromUmbraco(RenderModel model, List<AlertViewModel> alerts)
        {
            foreach (var alertPage in model.Content.Children)
            {
                var alertModel = new AlertViewModel()
                {
                    Alert = new HtmlString(alertPage.GetPropertyValue<string>("alert")),
                    TargetUrls = GetTargetUrls(alertPage),
                    Append = alertPage.GetPropertyValue<bool>("append"),
                    Cascade = alertPage.GetPropertyValue<bool>("cascade")
                };

                if (alertModel.TargetUrls.Count > 0)
                {
                    alerts.Add(alertModel);
                }
            }
        }

        private static IList<Uri> GetTargetUrls(IPublishedContent alertPage)
        {
            var list = new List<Uri>();

            // Get URLs from multi-node tree picker
            var multiNodeTreePicker = alertPage.GetPropertyValue<IEnumerable<IPublishedContent>>("whereToDisplayIt");
            if (multiNodeTreePicker != null)
            {
                foreach (var targetUrl in multiNodeTreePicker)
                {
                    try
                    {
                        list.Add(new Uri(targetUrl.Url, UriKind.RelativeOrAbsolute));
                    }
                    catch (UriFormatException)
                    {
                    }
                }
            }

            // Get URLs from textbox
            var urlList = alertPage.GetPropertyValue<string>("whereElseToDisplayIt");
            if (!String.IsNullOrWhiteSpace(urlList))
            {
                var urlLines = urlList.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var urlLine in urlLines)
                {
                    try
                    {
                        var url = new Uri(urlLine, UriKind.RelativeOrAbsolute);
                        if (url.IsAbsoluteUri) url = new Uri(url.PathAndQuery, UriKind.Relative);
                        list.Add(url);
                    }
                    catch (UriFormatException)
                    {
                    }
                }
            }
            return list;
        }
    }
}
