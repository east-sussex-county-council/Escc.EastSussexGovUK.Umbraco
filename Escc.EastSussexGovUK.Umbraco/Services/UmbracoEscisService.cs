using System;
using Escc.EastSussexGovUK.Features;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Service to control use of the ESCIS search
    /// </summary>
    public class UmbracoEscisService : IEscisService
    {
        private IPublishedContent content;

        /// <summary>
        /// Creates a new instance of <see cref="UmbracoEscisService"/>
        /// </summary>
        /// <param name="content"></param>
        public UmbracoEscisService(IPublishedContent content)
        {
            this.content = content;
        }

        /// <summary>
        /// Determines whether to show the ESCIS search based on a setting from the current page and its parents
        /// </summary>
        /// <returns></returns>
        public bool ShowSearch()
        {
            return ShowSearchBasedOnUmbracoContent(this.content);
        }

        private static bool ShowSearchBasedOnUmbracoContent(IPublishedContent contentNode)
        {
            // This is a recursive method which can reach the top of the content tree. If that happens, assume false.
            if (contentNode == null) return false;

            // Get the selected value. If none set, use INHERIT.
            var oneSpace = umbraco.library.GetPreValueAsString(contentNode.GetPropertyValue<int>("escis_Social_media_and_promotion"));
            oneSpace = String.IsNullOrEmpty(oneSpace) ? "INHERIT" : oneSpace.ToUpperInvariant();

            // If set to inherit, look to the parent page for the setting.
            if (oneSpace == "INHERIT") return ShowSearchBasedOnUmbracoContent(contentNode.Parent);

            // Otherwise take the setting from this page
            return (oneSpace == "SHOW");
        }
    }
}