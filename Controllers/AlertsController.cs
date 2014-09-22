using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Models;
using EsccWebTeam.Data.Web;
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

            // Cache for speed, but not for long because content is time-sensitive
            Http.CacheFor(0, 5);

            return CurrentTemplate(alerts);
        }

        private static void AddAlertsFromUmbraco(RenderModel model, List<AlertViewModel> alerts)
        {
            foreach (var alertPage in model.Content.Children)
            {
                var multiNodeTreePicker = alertPage.GetPropertyValue<IEnumerable<IPublishedContent>>("whereToDisplayIt");
                if (multiNodeTreePicker == null) continue;

                var alertModel = new AlertViewModel()
                {
                    Alert = new HtmlString(alertPage.GetPropertyValue<string>("alert")),
                    TargetUrls = new List<Uri>(),
                    Append = alertPage.GetPropertyValue<bool>("append"),
                    Cascade = alertPage.GetPropertyValue<bool>("cascade")
                };

                foreach (var targetUrl in multiNodeTreePicker)
                {
                    alertModel.TargetUrls.Add(new Uri(targetUrl.Url, UriKind.RelativeOrAbsolute));
                }

                alerts.Add(alertModel);
            }
        }
    }
}
