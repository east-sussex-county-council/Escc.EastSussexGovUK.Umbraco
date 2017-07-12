using System;
using System.Text.RegularExpressions;
using Exceptionless;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// If a 'Job alert' is requested with the /jobalertid suffix on the URL, match the job alert
    /// </summary>
    public class JobAlertContentFinder : IContentFinder
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

                var encoder = new JobAlertIdEncoder();
                var alertId = encoder.ParseIdFromUrl(contentRequest.Uri);
                if (String.IsNullOrEmpty(alertId))
                {
                    return false; // not found
                }

                // if we remove the suffix, does that match a Job Alerts node?
                var contentCache = UmbracoContext.Current.ContentCache;
                var content = contentCache.GetByRoute(encoder.RemoveIdFromUrl(contentRequest.Uri).AbsolutePath);
                if (content == null) return false; // not found
                if (content.ContentType.Alias != "JobAlerts") return false; // not found

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