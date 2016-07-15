using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.UmbracoViews.Services;
using Escc.EastSussexGovUK.UmbracoViews.ViewModels;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using CampaignContentViewModel = Escc.EastSussexGovUK.Umbraco.Models.CampaignContentViewModel;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    /// <summary>
    /// A content page for marketing campaigns
    /// </summary>
    public class CampaignContentController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var viewModel = MapUmbracoContentToViewModel(model.Content, new ContentExperimentSettingsService());

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private static CampaignContentViewModel MapUmbracoContentToViewModel(IPublishedContent content, IContentExperimentSettingsService contentExperimentSettingsService)
        {
            var model = new CampaignContentViewModel();

            model.ContentPart1 = new HtmlString(content.GetPropertyValue<string>("Content1_Content"));
            model.ContentPart2 = new HtmlString(content.GetPropertyValue<string>("Content2_Content"));
            model.ContentPart3 = new HtmlString(content.GetPropertyValue<string>("Content3_Content"));
            model.ContentPart4 = new HtmlString(content.GetPropertyValue<string>("Content4_Content"));

            model.UpperQuote = content.GetPropertyValue<string>("UpperQuote_Content");
            model.CentralQuote = content.GetPropertyValue<string>("CentralQuote_Content");
            model.LowerQuote = content.GetPropertyValue<string>("LowerQuote_Content");
            model.FinalQuote = content.GetPropertyValue<string>("FinalQuote_Content");

            model.PullQuoteBackgroundColour = content.GetPropertyValue<string>("PullQuoteBackground_Design");
            model.PullQuoteQuotationMarksColour = content.GetPropertyValue<string>("PullQuoteMarks_Design");
            model.CentralQuoteBackgroundColour = content.GetPropertyValue<string>("CentralQuoteBackground_Design");
            model.FinalQuoteTextColour = content.GetPropertyValue<string>("FinalQuoteColour_Design");

            var imageData = content.GetPropertyValue<IPublishedContent>("BannerImageSmall_Design");
            if (imageData != null)
            {
                model.BannerImageSmall = new Image()
                {
                    ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            imageData = content.GetPropertyValue<IPublishedContent>("BannerImageLarge_Design");
            if (imageData != null)
            {
                model.BannerImageLarge = new Image()
                {
                    ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            imageData = content.GetPropertyValue<IPublishedContent>("CentralQuoteImage_Content");
            if (imageData != null)
            {
                model.CentralQuoteImage = new Image()
                {
                    ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative)),
                    AlternativeText = imageData.Name
                };
            }
            imageData = content.GetPropertyValue<IPublishedContent>("FinalQuoteImage_Content");
            if (imageData != null)
            {
                model.FinalQuoteImage = new Image()
                {
                    ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative)),
                    AlternativeText = imageData.Name
                };
            }

            model.CustomCssLargeScreen = new HtmlString(content.GetPropertyValue<string>("CssLarge_Design"));
            
            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService, UmbracoContext.Current.InPreviewMode);

            // Add sibling pages
            foreach (var sibling in content.Siblings<IPublishedContent>())
            {
                model.CampaignPages.Add(new GuideNavigationLink()
                {
                    Text = sibling.Name,
                    Url =  new Uri(sibling.Url, UriKind.Relative),
                    IsCurrentPage = (sibling.Id == content.Id)
                });
            }


            return model;
        }
    }
}