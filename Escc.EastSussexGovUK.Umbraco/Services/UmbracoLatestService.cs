using System;
using System.Text;
using System.Web;
using Escc.Dates;
using Escc.EastSussexGovUK.Features;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Service to aggregate 'latest' content from Umbraco pages
    /// </summary>
    public class UmbracoLatestService : ILatestService
    {
        private IPublishedContent content;

        /// <summary>
        /// Creates a new instance of <see cref="UmbracoLatestService"/>
        /// </summary>
        /// <param name="content"></param>
        public UmbracoLatestService(IPublishedContent content)
        {
            this.content = content;
        }
        
        /// <summary>
        /// Gather 'latest' content from the current page and its ancestors
        /// </summary>
        /// <param name="content"></param>
        /// <param name="latestHtml"></param>
        /// <param name="isRecursiveRequest"></param>
        private static void GetLatestRecursive(IPublishedContent content, StringBuilder latestHtml, bool isRecursiveRequest)
        {
            // This is a recursive method. When there are no more ancestors to recurse, return.
            if (content == null) return;

            var latest = BuildLatestModel(content);

            // Does the document type being recursed over have the "latest" properties?
            var documentTypeSupportsLatest = (latest != null);
            if (documentTypeSupportsLatest)
            {
                var now = DateTime.Now.ToUkDateTime();
                if (!String.IsNullOrWhiteSpace(latest.LatestHtml.ToHtmlString())
                    && (!latest.PublishDate.HasValue || now >= latest.PublishDate)
                    && (!latest.UnpublishDate.HasValue || now <= latest.UnpublishDate)
                    && (!isRecursiveRequest || latest.Cascade)
                    )
                {
                    // If this is the current page or one that allows cascades, and it has a latest 
                    // which is allowed to be displayed today, add it to our result.
                    latestHtml.Insert(0, latest.LatestHtml);
                }
            }

            if (!documentTypeSupportsLatest || latest.Inherit)
            {
                // If inheritance is allowed, keep looking up the tree
                GetLatestRecursive(content.Parent, latestHtml, true);
            }

        }

        /// <summary>
        /// For a single Umbraco document, get the properties for the 'latest'
        /// </summary>
        /// <param name="content"></param>
        /// <returns>A <see cref="LatestSettings"/>, or <c>null</c> if the properties are not present</returns>
        private static LatestSettings BuildLatestModel(IPublishedContent content)
        {
            var latestHtml = content.GetPropertyValue<string>("latest_Latest");
            if (latestHtml == null) return null;

            var model = new LatestSettings()
            {
                LatestHtml = new HtmlString(latestHtml)
            };

            // Latests can be published before or until given dates.
            // Unpublish date is more intuitive if it's until midnight at the end of the day, not the start, so add a day.
            var publish = content.GetPropertyValue<DateTime>("latestPublishDate_Latest");
            if (publish != DateTime.MinValue) model.PublishDate = publish;

            var unpublish = content.GetPropertyValue<DateTime>("latestUnpublishDate_Latest");
            if (unpublish != DateTime.MinValue) model.UnpublishDate = unpublish.AddDays(1);

            // At each level editors can select whether to allow recursion up to parent pages or cascading down to children.
            model.Inherit = content.GetPropertyValue<bool>("latestInherit_Latest");
            model.Cascade = content.GetPropertyValue<bool>("latestCascade_Latest");

            return model;
        }

        public LatestSettings ReadLatestSettings()
        {
            var latestHtml = new StringBuilder();
            GetLatestRecursive(this.content, latestHtml, false);
            return new LatestSettings()
            {
                LatestHtml = new HtmlString(latestHtml.ToString())
            };
        }
    }
}