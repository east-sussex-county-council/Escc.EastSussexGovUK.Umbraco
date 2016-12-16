using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Builds related links from a related links field, and converts media URLs to Azure blob storage URLs
    /// </summary>
    /// <seealso cref="Escc.Umbraco.PropertyTypes.IRelatedLinksService" />
    public class UmbracoOnAzureRelatedLinksService : IRelatedLinksService
    {
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoOnAzureRelatedLinksService"/> class.
        /// </summary>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        public UmbracoOnAzureRelatedLinksService(IMediaUrlTransformer mediaUrlTransformer)
        {
            if (mediaUrlTransformer == null) throw new ArgumentNullException(nameof(mediaUrlTransformer));
            _mediaUrlTransformer = mediaUrlTransformer;
        }

        /// <summary>
        /// Builds the content of the related links view model from Umbraco.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="propertyAlias">The property alias.</param>
        /// <returns></returns>
        public IList<HtmlLink> BuildRelatedLinksViewModelFromUmbracoContent(IPublishedContent content, string propertyAlias)
        {
            var baseService = new RelatedLinksService();
            var links = baseService.BuildRelatedLinksViewModelFromUmbracoContent(content, propertyAlias);
            links = new List<HtmlLink>(links);
            foreach (var link in links)
            {
                if (link.Url != null)
                {
                    link.Url = _mediaUrlTransformer.TransformMediaUrl(link.Url);
                }
            }

            return links;
        }
    }
}