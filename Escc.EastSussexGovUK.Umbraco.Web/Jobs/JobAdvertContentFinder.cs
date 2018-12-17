using System;
using System.Text.RegularExpressions;
using Exceptionless;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    /// <summary>
    /// If a 'Job advert' is requested with the /jobid/job-title suffix on the URL, match the job advert
    /// </summary>
    public class JobAdvertContentFinder : IContentFinder
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
                if (contentRequest == null) throw new ArgumentNullException(nameof(contentRequest));

                var path = contentRequest.Uri.GetAbsolutePathDecoded();
                var jobUrlSegment = Regex.Match(path, "/[0-9]+/");
                if (!jobUrlSegment.Success)
                {
                    return false; // not found
                }

                // if we remove the /job/job-title suffix, does that match a Job Advert node?
                var contentCache = UmbracoContext.Current.ContentCache;
                var content = contentCache.GetByRoute(path.Substring(0, jobUrlSegment.Groups[0].Index));
                if (content == null) return false; // not found
                if (content.ContentType.Alias != "JobAdvert") return false; // not found

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