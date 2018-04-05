using System;
using System.Collections.Generic;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Guide;

namespace Escc.EastSussexGovUK.Umbraco.CampaignTemplates
{
    /// <summary>
    /// Creates a <see cref="CampaignTilesViewModel"/> from an instance of its Umbraco document type
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{CampaignTilesViewModel}" />
    public class CampaignTilesViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<CampaignTilesViewModel>
    {
        private readonly IPublishedContent _umbracoContent;
        private readonly IRelatedLinksService _relatedLinksService;
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignTilesViewModelFromUmbraco" /> class.
        /// </summary>
        /// <param name="umbracoContent">The content.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <exception cref="System.ArgumentNullException">umbracoContent
        /// or
        /// mediaUrlTransformer</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CampaignTilesViewModelFromUmbraco(IPublishedContent umbracoContent, IRelatedLinksService relatedLinksService, IMediaUrlTransformer mediaUrlTransformer) : base(umbracoContent, relatedLinksService, mediaUrlTransformer)
        {
            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));
            if (relatedLinksService == null) throw new ArgumentNullException(nameof(relatedLinksService));
            if (mediaUrlTransformer == null) throw new ArgumentNullException(nameof(mediaUrlTransformer));

            _umbracoContent = umbracoContent;
            _relatedLinksService = relatedLinksService;
            _mediaUrlTransformer = mediaUrlTransformer;
        }

        public CampaignTilesViewModel BuildModel()
        {
            var model = new CampaignTilesViewModel();

            var tileImages = BuildImages("TileImages_Content");
            var tileNavigation = _relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(_umbracoContent, "TileNavigation_Content");

            var total = tileImages.Count > tileNavigation.Count ? tileImages.Count : tileNavigation.Count;

            for (var i=0; i < total; i++)
            {
                var image = (i < tileImages.Count) ? tileImages[i] : null;
                var title = (i < tileNavigation.Count) ? tileNavigation[i].Text : null;
                var url = (i < tileNavigation.Count) ? tileNavigation[i].Url : null;
                var description = (i < 12) ? _umbracoContent.GetPropertyValue<string>($"Tile{i+1}Description_Content") : null;

                model.Tiles.Add(new CampaignTile()
                {
                    Image = image,
                    Title = title,
                    Url = url,
                    Description = description
                });
            }

            model.BannerImageSmall = BuildImage("BannerImageSmall_Design");
            model.BannerImageLarge = BuildImage("BannerImageLarge_Design");

            model.TileFontFamily = _umbracoContent.GetPropertyValue<string>("TileFontFamily_Design");
            model.TileTitleTextColour = _umbracoContent.GetPropertyValue<string>("TileTitleColour_Design");
            model.TileDescriptionsTextColour = _umbracoContent.GetPropertyValue<string>("TileDescriptionColour_Design");

            model.CustomCssSmallScreen = new HtmlString(_umbracoContent.GetPropertyValue<string>("CssSmall_Design"));
            model.CustomCssMediumScreen = new HtmlString(_umbracoContent.GetPropertyValue<string>("CssMedium_Design"));
            model.CustomCssLargeScreen = new HtmlString(_umbracoContent.GetPropertyValue<string>("CssLarge_Design"));

            // Add sibling pages
            foreach (var sibling in _umbracoContent.Siblings<IPublishedContent>())
            {
                model.CampaignPages.Add(new GuideNavigationLink()
                {
                    Text = sibling.Name,
                    Url = new Uri(sibling.Url, UriKind.Relative),
                    IsCurrentPage = (sibling.Id == _umbracoContent.Id)
                });
            }


            return model;
        }
    }
}