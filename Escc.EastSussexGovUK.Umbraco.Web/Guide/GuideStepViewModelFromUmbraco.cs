using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Services;

namespace Escc.EastSussexGovUK.Umbraco.Web.Guide
{
    /// <summary>
    /// Builds a <see cref="GuideStepViewModel"/> from an Umbraco node based on <see cref="GuideStepDocumentType"/>
    /// </summary>
    /// <seealso cref="GuideStepViewModel" />
    public class GuideStepViewModelFromUmbraco : IViewModelBuilder<GuideStepViewModel>
    {
        private readonly IPublishedContent _content;
        private readonly IRelatedLinksService _relatedLinksService;
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuideStepViewModelFromUmbraco" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <exception cref="System.ArgumentNullException">content
        /// or
        /// relatedLinksService
        /// or
        /// mediaUrlTransformer</exception>
        public GuideStepViewModelFromUmbraco(IPublishedContent content, IRelatedLinksService relatedLinksService, IMediaUrlTransformer mediaUrlTransformer)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (relatedLinksService == null) throw new ArgumentNullException(nameof(relatedLinksService));
            if (mediaUrlTransformer == null) throw new ArgumentNullException(nameof(mediaUrlTransformer));

            _content = content;
            _relatedLinksService = relatedLinksService;
            _mediaUrlTransformer = mediaUrlTransformer;
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        public GuideStepViewModel BuildModel()
        {
            var model = new GuideStepViewModel()
            {
                GuideUrl = new Uri(_content.Parent.Url, UriKind.RelativeOrAbsolute),
                GuideTitle = _content.Parent.Name,
                StepContent = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_content.GetPropertyValue<string>("content_Content")))
            };

            model.Metadata.IsInSearch = !_content.GetPropertyValue<bool>("hideFromSearchEngines");

            var sectionNavigation = _content.Parent.GetPropertyValue<int>("SectionNavigation_Navigation");
            model.StepsHaveAnOrder = (sectionNavigation == 0 || umbraco.library.GetPreValueAsString(sectionNavigation).ToUpperInvariant() != "BULLETED LIST");

            var relatedLinksGroups = new RelatedLinksModelBuilder().OrganiseAsHeadingsAndSections(_relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(_content, "relatedLinks_Content"));
            foreach (var linkGroup in relatedLinksGroups)
            {
                model.RelatedLinksGroups.Add(linkGroup);
            }

            model.Steps = new List<GuideNavigationLink>(_content.Siblings<IPublishedContent>()
                .Where(sibling => sibling.ContentType == _content.ContentType)
                .Select(sibling => new GuideNavigationLink()
                {
                    Text = sibling.Name,
                    Url = new Uri(sibling.Url, UriKind.Relative),
                    IsCurrentPage = (sibling.Id == _content.Id)
                }));


            var partnerImages = _content.GetPropertyValue<IEnumerable<IPublishedContent>>("partnerImages_Content");
            foreach (var imageData in partnerImages)
            {
                var image = new Image()
                {
                    AlternativeText = imageData.Name,
                    ImageUrl = new Uri(imageData.Url, UriKind.Relative),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
                model.PartnerImages.Add(image);
            }

            return model;
        }
    }
}