using System;
using System.Web.Hosting;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Escc.EastSussexGovUK.Umbraco.Errors
{
    /// <summary>
    /// Read settings from the httpErrors section of web.config
    /// </summary>
    internal class HttpStatusFromConfiguration
    {
        /// <summary>
        /// Gets the custom URL configured for a status code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns></returns>
        internal Uri GetCustomUrlForStatusCode(int statusCode)
        {
            var config = XElement.Load(HostingEnvironment.MapPath("~") + "web.config");
            var configNav = config.CreateNavigator();
            var notFoundErrorConfig = configNav.SelectSingleNode($"system.webServer/httpErrors/error[@statusCode='{statusCode}']");
            if (notFoundErrorConfig != null)
            {
                return new Uri(notFoundErrorConfig.GetAttribute("path", String.Empty), UriKind.RelativeOrAbsolute);
            }
            return null;
        }
    }
}