using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;

namespace Escc.EastSussexGovUK.Umbraco.Web.TextSize
{
    /// <summary>
    /// Register the route to <see cref="TextSizeController"/>
    /// </summary>
    /// <seealso cref="Umbraco.Core.ApplicationEventHandler" />
    public class RegisterTextSizeRoute : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            RouteTable.Routes.MapRoute(
                "TextSize",
                "TextSize/{action}",
                new
                {
                    controller = "TextSize",
                    action = "Change"
                });
        }
    }
}