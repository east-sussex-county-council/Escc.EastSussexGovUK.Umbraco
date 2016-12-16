using System;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Base class for building jobs view models from Umbraco content
    /// </summary>
    public abstract class BaseJobsViewModelFromUmbracoBuilder
    {
        protected IPublishedContent UmbracoContent { get; }
        protected IRelatedLinksService RelatedLinksService { get; }

        /// <summary>
        /// Initializes a new instance of a type derived from <see cref="BaseJobsViewModelFromUmbracoBuilder" />.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        protected BaseJobsViewModelFromUmbracoBuilder(IPublishedContent umbracoContent, IRelatedLinksService relatedLinksService=null)
        {
            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));

            UmbracoContent = umbracoContent;
            RelatedLinksService = relatedLinksService;
        }

        protected HtmlLink BuildLinkToPage(string alias)
        {
            var linkedPage = UmbracoContent.GetPropertyValue<IPublishedContent>(alias);
            if (linkedPage != null)
            {
                return new HtmlLink()
                {
                    Text = linkedPage.Name,
                    Url = new Uri(linkedPage.UrlAbsolute())
                };
            }
            return null;
        }

        protected IList<Image> BuildImages(string alias)
        {
            var imagesData = UmbracoContent.GetPropertyValue<IEnumerable<IPublishedContent>>(alias);
            var images = new List<Image>();
            foreach (var imageData in imagesData)
            {
                images.Add(BuildImageFromMediaItem(imageData));
            }
            return images;
        }

        protected Image BuildImage(string alias)
        {
            var imageData = UmbracoContent.GetPropertyValue<IPublishedContent>(alias);
            if (imageData == null) return null;
            return BuildImageFromMediaItem(imageData);
        }

        private Image BuildImageFromMediaItem(IPublishedContent imageData)
        {
            var image = new Image()
            {
                AlternativeText = imageData.Name,
                ImageUrl = new Uri(imageData.Url, UriKind.Relative),
                Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                Height = imageData.GetPropertyValue<int>("umbracoHeight")
            };
            return image;
        }
    }
}