using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Creates a <see cref="CampaignLandingViewModel"/> from an instance of its document type in Umbraco
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{CampaignLandingViewModel}" />
    public class CampaignLandingViewModelFromUmbraco : IViewModelBuilder<CampaignLandingViewModel>
    {
        private readonly IPublishedContent _umbracoContent;
        private readonly IRelatedLinksService _relatedLinksService;
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignLandingViewModelFromUmbraco"/> class.
        /// </summary>
        /// <param name="umbracoContent">The content.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public CampaignLandingViewModelFromUmbraco(IPublishedContent umbracoContent, IRelatedLinksService relatedLinksService, IMediaUrlTransformer mediaUrlTransformer)
        {
            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));
            if (relatedLinksService == null) throw new ArgumentNullException(nameof(relatedLinksService));
            if (mediaUrlTransformer == null) throw new ArgumentNullException(nameof(mediaUrlTransformer));

            _umbracoContent = umbracoContent;
            _relatedLinksService = relatedLinksService;
            _mediaUrlTransformer = mediaUrlTransformer;
        }

        public CampaignLandingViewModel BuildModel()
        {
            var model = new CampaignLandingViewModel();

            model.Introduction = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("Introduction_Content")));

            model.LandingNavigation.Sections = BuildLandingLinksViewModelFromUmbracoContent(_umbracoContent, _relatedLinksService);

            var buttonLinks = _relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(_umbracoContent, "ButtonNavigation_Content");

            for (var i = 0; i <= 2 && i < buttonLinks.Count; i++)
            {
                var target = buttonLinks[i];
                if (target != null)
                {
                    model.ButtonTargets.Add(target);
                    model.ButtonDescriptions.Add(_umbracoContent.GetPropertyValue<string>("Button" + (i + 1) + "Description_Content"));
                }
            }

            model.ButtonsTopMarginSmall = _umbracoContent.GetPropertyValue<int?>("ButtonsTopMarginSmall_Design");
            model.ButtonsTopMarginMedium = _umbracoContent.GetPropertyValue<int?>("ButtonsTopMarginMedium_Design");
            model.ButtonsTopMarginLarge = _umbracoContent.GetPropertyValue<int?>("ButtonsTopMarginLarge_Design");

            model.Content = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("Content_Content")));

            var imageData = _umbracoContent.GetPropertyValue<IPublishedContent>("BackgroundSmall_Design");
            if (imageData != null)
            {
                model.BackgroundImageSmall = new Image()
                {
                    ImageUrl = _mediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            imageData = _umbracoContent.GetPropertyValue<IPublishedContent>("BackgroundMedium_Design");
            if (imageData != null)
            {
                model.BackgroundImageMedium = new Image()
                {
                    ImageUrl = _mediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            imageData = _umbracoContent.GetPropertyValue<IPublishedContent>("BackgroundLarge_Design");
            if (imageData != null)
            {
                model.BackgroundImageLarge = new Image()
                {
                    ImageUrl = _mediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            model.BackgroundColour = _umbracoContent.GetPropertyValue<string>("BackgroundColour_Design");
            model.BackgroundImageWrapsHorizontally = _umbracoContent.GetPropertyValue<bool>("BackgroundImageWrapsHorizontally_Design");
            model.BackgroundImageWrapsVertically = _umbracoContent.GetPropertyValue<bool>("BackgroundImageWrapsVertically_Design");

            model.CustomCssSmallScreen = new HtmlString(_umbracoContent.GetPropertyValue<string>("CssSmall_Design"));
            model.CustomCssMediumScreen = new HtmlString(_umbracoContent.GetPropertyValue<string>("CssMedium_Design"));
            model.CustomCssLargeScreen = new HtmlString(_umbracoContent.GetPropertyValue<string>("CssLarge_Design"));

            model.ButtonsHorizontalAtMedium = _umbracoContent.GetPropertyValue<bool>("ButtonsHorizontalAtMedium_Design");

            model.VideoHeight = _umbracoContent.GetPropertyValue<int?>("VideoHeight_Design");

            return model;
        }


        /// <summary>
        /// For each related link, create a <see cref="LandingSectionViewModel" />
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <returns></returns>
        private static IList<LandingSectionViewModel> BuildLandingLinksViewModelFromUmbracoContent(IPublishedContent content, IRelatedLinksService relatedLinksService)
        {
            var sections = new List<LandingSectionViewModel>() { };
            var relatedLinks = relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(content, "LandingNavigation_Content");
            foreach (var link in relatedLinks)
            {
                var section = new LandingSectionViewModel()
                {
                    Heading = link,
                    Links = new HtmlLink[0]
                };
                sections.Add(section);
            }
            return sections;
        }
    }
}