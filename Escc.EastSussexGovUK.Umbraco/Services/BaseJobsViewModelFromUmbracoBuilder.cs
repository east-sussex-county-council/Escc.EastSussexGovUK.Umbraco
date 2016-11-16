using System;
using System.Collections.Generic;
using AST.AzureBlobStorage.Helper;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseJobsViewModelFromUmbracoBuilder
    {
        protected IPublishedContent UmbracoContent { get; }
        protected IRelatedLinksService RelatedLinksService { get; }
        protected IMediaUrlTransformer MediaUrlTransformer { get; }

        /// <summary>
        /// Initializes a new instance of a type derived from <see cref="BaseJobsViewModelFromUmbracoBuilder" />.
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        protected BaseJobsViewModelFromUmbracoBuilder(IPublishedContent umbracoContent, IRelatedLinksService relatedLinksService, IMediaUrlTransformer mediaUrlTransformer)
        {
            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));
            if (relatedLinksService == null) throw new ArgumentNullException(nameof(relatedLinksService));
            if (mediaUrlTransformer == null) throw new ArgumentNullException(nameof(mediaUrlTransformer));

            UmbracoContent = umbracoContent;
            RelatedLinksService = relatedLinksService;
            MediaUrlTransformer = mediaUrlTransformer;
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
            return BuildImageFromMediaItem(imageData);
        }

        private Image BuildImageFromMediaItem(IPublishedContent imageData)
        {
            var image = new Image()
            {
                AlternativeText = imageData.Name,
                ImageUrl = MediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative)),
                Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                Height = imageData.GetPropertyValue<int>("umbracoHeight")
            };
            return image;
        }
    }
}