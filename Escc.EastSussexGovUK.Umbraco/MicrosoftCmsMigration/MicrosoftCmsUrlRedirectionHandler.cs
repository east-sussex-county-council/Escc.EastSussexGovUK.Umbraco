using Exceptionless;
using System;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration
{
    public class MicrosoftCmsUrlRedirectionHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var requestedUrl = context.Request.Url;
                if (requestedUrl.AbsolutePath.EndsWith(".htm", StringComparison.OrdinalIgnoreCase) && !requestedUrl.AbsolutePath.ToUpperInvariant().Contains("APP_PLUGINS"))
                {
                    // The default page in a Microsoft CMS channel was usually called default.htm, and we took the channel name as the Umbraco page name,
                    // so strip /default.htm from the URL to get back to the URL for its channel. For other pages, just remove the .htm extension
                    var suffixToRemove = requestedUrl.AbsolutePath.EndsWith("/default.htm", StringComparison.OrdinalIgnoreCase) ? 11 : 4;
                    var rewriteUrl = requestedUrl.AbsolutePath.Substring(0, requestedUrl.AbsolutePath.Length - suffixToRemove);
                    if (requestedUrl.Query.Length > 0) rewriteUrl += requestedUrl.Query;
                    context.Response.RedirectPermanent(rewriteUrl);
                }
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }
        }

        #endregion
    }
}
