using System;
using Exceptionless;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Escc.EastSussexGovUK.Umbraco.Guide
{
    /// <summary>
    /// If a Guide is requested with the /print suffix on the URL, match the Guide
    /// </summary>
    public class GuidePrintViewContentFinder : IContentFinder
    {
        /// <summary>
        /// Tries to find the content
        /// </summary>
        /// <param name="contentRequest">The request</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            try
            {
                if (contentRequest == null) throw new ArgumentNullException("contentRequest");

                var path = contentRequest.Uri.GetAbsolutePathDecoded();
                if (!path.EndsWith("/print", StringComparison.OrdinalIgnoreCase))
                {
                    return false; // not found
                }

                // if we remove the /print suffix, does that match a Guide node?
                var contentCache = UmbracoContext.Current.ContentCache;
                var content = contentCache.GetByRoute(path.TrimEnd("/print"));
                if (content == null) return false; // not found
                if (content.ContentType.Alias != "Guide") return false; // not found

                // render that node
                contentRequest.PublishedContent = content;
                return true;
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return false;
            } 
 
    }
    }
}