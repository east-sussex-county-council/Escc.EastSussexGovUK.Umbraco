using System;
using System.Web;
using Exceptionless;
using Umbraco.Web.Routing;

namespace Escc.EastSussexGovUK.Umbraco.Web.Errors
{
    /// <summary>
    /// Deal with 404 errors within Umbraco by passing the URL not found to the 404 page configured in IIS
    /// </summary>
    public class NotFoundContentFinder : IContentFinder
    {
        /// <summary>
        /// Tries to find and assign an Umbraco document to a <c>PublishedContentRequest</c>.
        /// </summary>
        /// <param name="contentRequest">The <c>PublishedContentRequest</c>.</param>
        /// <returns>
        /// A value indicating whether an Umbraco document was found and assigned.
        /// </returns>
        /// <remarks>
        /// Optionally, can also assign the template or anything else on the document request, although that is not required.
        /// </remarks>
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            if (contentRequest == null) throw new ArgumentNullException(nameof(contentRequest));
            try
            {
                // Hand over to 404 page configured for IIS, including the requested URL in the format used by IIS
                var errorUrl = new HttpStatusFromConfiguration().GetCustomUrlForStatusCode(404);
                if (errorUrl != null)
                {
                    var requestUrl = contentRequest.RoutingContext.UmbracoContext.HttpContext.Request.Url;
                    contentRequest.RoutingContext.UmbracoContext.HttpContext.Server.TransferRequest(errorUrl + "?404;" + HttpUtility.UrlEncode(requestUrl.ToString()));
                }
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }

            // If we get this far, returning false displays the "this page is intentionally ugly" Umbraco 404. 
            // Returning true causes a 500 "There is no PublishedContent."
            return false;
        }
    }
}