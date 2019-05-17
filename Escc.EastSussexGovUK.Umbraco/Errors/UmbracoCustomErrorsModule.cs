using System;
using System.Collections.Generic;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Errors
{
    /// <summary>
    /// HTTP module which implements the custom errors settings configured in IIS when running under Umbraco but accessing URLs that end with extensions not normally processed by Umbraco, such as .js or .html
    /// </summary>
    /// <seealso cref="System.Web.IHttpModule" />
    public class UmbracoCustomErrorsModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            // Handle requests that resulted in a 404
            var notFoundUrl = new HttpStatusFromConfiguration().GetCustomUrlForStatusCode(404);
            if (notFoundUrl != null && HttpContext.Current != null && HttpContext.Current.Response.StatusCode == 404)
            {
                // If a URL was requested that would not have been handled by Umbraco, pass it to the configured 404 page.
                // Check for the 404 page itself because the request for the 404 comes back here and needs to be ignored.
                var pathRequested = HttpContext.Current.Request.Url.AbsolutePath;
                var extensionRequested = VirtualPathUtility.GetExtension(pathRequested);
                var extensionsHandledByUmbraco = new List<string> {string.Empty, ".aspx"};
                if (!pathRequested.StartsWith(notFoundUrl.ToString()) && !extensionsHandledByUmbraco.Contains(extensionRequested))
                {
                    HttpContext.Current.Server.TransferRequest(notFoundUrl + "?404;" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.ToString()));
                }
            }
        }

        #endregion

    }
}
