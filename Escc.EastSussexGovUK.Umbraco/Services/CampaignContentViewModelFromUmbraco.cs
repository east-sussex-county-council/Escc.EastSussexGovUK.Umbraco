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
    /// Creates a <see cref="CampaignContentViewModel"/> from an instance of its Umbraco document type
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{CampaignContentViewModel}" />
    public class CampaignContentViewModelFromUmbraco : IViewModelBuilder<CampaignContentViewModel>
    {
        private readonly IPublishedContent _umbracoContent;
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignContentViewModelFromUmbraco"/> class.
        /// </summary>
        /// <param name="umbracoContent">The content.</param>
        /// <param name="mediaUrlTransformer">The media URL transformer.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public CampaignContentViewModelFromUmbraco(IPublishedContent umbracoContent, IMediaUrlTransformer mediaUrlTransformer)
        {
            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));
            if (mediaUrlTransformer == null) throw new ArgumentNullException(nameof(mediaUrlTransformer));

            _umbracoContent = umbracoContent;
            _mediaUrlTransformer = mediaUrlTransformer;
        }

        public CampaignContentViewModel BuildModel()
        {
            var model = new CampaignContentViewModel();

            model.ContentPart1 = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("Content1_Content")));
            model.ContentPart2 = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("Content2_Content")));
            model.ContentPart3 = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("Content3_Content")));
            model.ContentPart4 = new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(_umbracoContent.GetPropertyValue<string>("Content4_Content")));

            model.UpperQuote = _umbracoContent.GetPropertyValue<string>("UpperQuote_Content");
            model.CentralQuote = _umbracoContent.GetPropertyValue<string>("CentralQuote_Content");
            model.LowerQuote = _umbracoContent.GetPropertyValue<string>("LowerQuote_Content");
            model.FinalQuote = _umbracoContent.GetPropertyValue<string>("FinalQuote_Content");

            model.PullQuoteBackgroundColour = _umbracoContent.GetPropertyValue<string>("PullQuoteBackground_Design");
            model.PullQuoteQuotationMarksColour = _umbracoContent.GetPropertyValue<string>("PullQuoteMarks_Design");
            model.CentralQuoteBackgroundColour = _umbracoContent.GetPropertyValue<string>("CentralQuoteBackground_Design");
            model.FinalQuoteTextColour = _umbracoContent.GetPropertyValue<string>("FinalQuoteColour_Design");

            model.CentralQuoteImageIsCutout = _umbracoContent.GetPropertyValue<bool>("CentralQuoteImageIsCutout_Design");

            var imageData = _umbracoContent.GetPropertyValue<IPublishedContent>("BannerImageSmall_Design");
            if (imageData != null)
            {
                model.BannerImageSmall = new Image()
                {
                    ImageUrl = _mediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            imageData = _umbracoContent.GetPropertyValue<IPublishedContent>("BannerImageLarge_Design");
            if (imageData != null)
            {
                model.BannerImageLarge = new Image()
                {
                    ImageUrl = _mediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            imageData = _umbracoContent.GetPropertyValue<IPublishedContent>("CentralQuoteImage_Content");
            if (imageData != null)
            {
                model.CentralQuoteImage = new Image()
                {
                    ImageUrl = _mediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative)),
                    AlternativeText = imageData.Name
                };
            }
            imageData = _umbracoContent.GetPropertyValue<IPublishedContent>("FinalQuoteImage_Content");
            if (imageData != null)
            {
                model.FinalQuoteImage = new Image()
                {
                    ImageUrl = _mediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative)),
                    AlternativeText = imageData.Name
                };
            }

            model.CustomCssLargeScreen = new HtmlString(_umbracoContent.GetPropertyValue<string>("CssLarge_Design"));

            model.VideoWidth = _umbracoContent.GetPropertyValue<int?>("VideoWidth_Design");
            model.VideoHeight = _umbracoContent.GetPropertyValue<int?>("VideoHeight_Design");

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