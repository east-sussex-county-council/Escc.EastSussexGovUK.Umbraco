using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Services;

namespace Escc.EastSussexGovUK.Umbraco.Web.Task
{
    /// <summary>
    /// Builds a <see cref="TaskViewModel"/> from an Umbraco node based on <see cref="TaskDocumentType"/>
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder&lt;T&gt;" />
    public class TaskViewModelFromUmbraco : IViewModelBuilder<TaskViewModel>
    {
        private readonly IPublishedContent _umbracoContent;
        private readonly IRelatedLinksService _relatedLinksService;
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Builds a <see cref="TaskViewModel"/> from Umbraco content
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <param name="mediaUrlTransformer">A service to update links to items in the media library</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public TaskViewModelFromUmbraco(IPublishedContent umbracoContent, IRelatedLinksService relatedLinksService, IMediaUrlTransformer mediaUrlTransformer)
        {
            _umbracoContent = umbracoContent;
            _relatedLinksService = relatedLinksService;
            _mediaUrlTransformer = mediaUrlTransformer;

            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));
            if (relatedLinksService == null) throw new ArgumentNullException(nameof(relatedLinksService));
            if (mediaUrlTransformer == null) throw new ArgumentNullException(nameof(mediaUrlTransformer));
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <returns></returns>
        public TaskViewModel BuildModel()
        {
            var model = new TaskViewModel
            {
                LeadingText = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("leadingText_Content"))),
                StartPageUrl = _mediaUrlTransformer.TransformUrl(new Uri(_umbracoContent.GetPropertyValue<string>("startPageUrl_Content"), UriKind.RelativeOrAbsolute)),
                StartButtonText = _umbracoContent.GetPropertyValue<string>("startButtonText_Content"),
                Subheading1 = _umbracoContent.GetPropertyValue<string>("subheading1_Content"),
                Content1 = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("content1_Content"))),
                Subheading2 = _umbracoContent.GetPropertyValue<string>("subheading2_Content"),
                Content2 = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("content2_Content"))),
                Subheading3 = _umbracoContent.GetPropertyValue<string>("subheading3_Content"),
                Content3 = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("content3_Content"))),
                Subheading4 = _umbracoContent.GetPropertyValue<string>("subheading4_Content"),
                Content4 = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("content4_Content")))
            };

            var relatedLinksGroups = new RelatedLinksModelBuilder().OrganiseAsHeadingsAndSections(_relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(_umbracoContent, "relatedLinks_Content"));
            foreach (var linkGroup in relatedLinksGroups)
            {
                model.RelatedLinksGroups.Add(linkGroup);
            }

            var partnerImages = _umbracoContent.GetPropertyValue<IEnumerable<IPublishedContent>>("partnerImages_Content");
            foreach (var imageData in partnerImages)
            {
                var image = new Image()
                {
                    AlternativeText = imageData.Name,
                    ImageUrl = _mediaUrlTransformer.TransformUrl(new Uri(imageData.Url, UriKind.Relative)),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
                model.PartnerImages.Add(image);
            }

            return model;
        }
    }
}