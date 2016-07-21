using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AST.AzureBlobStorage.Helper;
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
                link.Url = ContentHelper.TransformUrl(link.Url);
            }

            return links;
        }
    }
}