using System;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration
{
    /// <summary>
    /// This Umbraco installtion will contain content migrated from Microsoft CMS 2002. This class eases the transition by making old URLs work.
    /// </summary>
    public class MicrosoftCmsCompatibleApplication : UmbracoApplication
    {
        /// <summary>
        /// At the start of each request, look for typical Microsoft CMS URLs and rewrite them to Umbraco URLs.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>An alternative approach would be to implement the Umbraco <see cref="Umbraco.Web.Routing.IContentFinder"/> interface, but although requests ending in .htm
        /// are routed to ASP.NET they are not routed to Umbraco, so content finders never get a chance to parse them.</remarks>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var requestedUrl = Request.Url.PathAndQuery;

            // The default page in a Microsoft CMS channel was usually called default.htm, and we took the channel name as the Umbraco page name,
            // so strip /default.htm from the URL to get back to the URL for its channel.
            if (requestedUrl.EndsWith("/default.htm"))
            {
                Context.RewritePath(requestedUrl.Substring(0, requestedUrl.Length - 12));
            }
            // For other pages, just remove the .htm extension
            else if (requestedUrl.EndsWith(".htm"))
            {
                Context.RewritePath(requestedUrl.Substring(0, requestedUrl.Length - 4));
            }
            // For both scenarios, it's slightly different if someone added a querystring.
            else if (requestedUrl.Contains("/default.htm?"))
            {
                Context.RewritePath(requestedUrl.Replace("/default.htm?", "?"));
            }
            else if (requestedUrl.Contains(".htm?"))
            {
                Context.RewritePath(requestedUrl.Replace(".htm?", "?"));
            }
        }
    }
}