using System;
using System.Threading;
using System.Web;
using Escc.Web;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration
{
    /// <summary>
    /// This Umbraco installation will contain content migrated from Microsoft CMS 2002. This class eases the transition by making old URLs work.
    /// </summary>
    /// <remarks>This HTTP module will run before Umbraco's content finders, although .htm requests are not passed to Umbraco anyway</remarks>
    public class MicrosoftCmsUrlRedirectionModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            if (context == null) throw new ArgumentNullException("context");

            context.BeginRequest += context_BeginRequest;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void context_BeginRequest(object sender, EventArgs args)
        {
            try
            {
                var requestedUrl = HttpContext.Current.Request.Url;
                if (requestedUrl.AbsolutePath.EndsWith(".htm", StringComparison.OrdinalIgnoreCase) && !requestedUrl.AbsolutePath.ToUpperInvariant().Contains("APP_PLUGINS"))
                {
                    // The default page in a Microsoft CMS channel was usually called default.htm, and we took the channel name as the Umbraco page name,
                    // so strip /default.htm from the URL to get back to the URL for its channel. For other pages, just remove the .htm extension
                    var suffixToRemove = requestedUrl.AbsolutePath.EndsWith("/default.htm", StringComparison.OrdinalIgnoreCase) ? 11 : 4;
                    var rewriteUrl = requestedUrl.AbsolutePath.Substring(0, requestedUrl.AbsolutePath.Length - suffixToRemove);
                    if (requestedUrl.Query.Length > 0) rewriteUrl += requestedUrl.Query;
                    new HttpStatus().MovedPermanently(rewriteUrl);
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
