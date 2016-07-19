using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using CampaignLandingViewModel = Escc.EastSussexGovUK.Umbraco.Models.CampaignLandingViewModel;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    /// <summary>
    /// A landing page for marketing campaigns
    /// </summary>
    public class CampaignLandingController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var viewModel = MapUmbracoContentToViewModel(model.Content, new ContentExperimentSettingsService(), new RelatedLinksService());

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private static CampaignLandingViewModel MapUmbracoContentToViewModel(IPublishedContent content, IContentExperimentSettingsService contentExperimentSettingsService, IRelatedLinksService relatedLinksService)
        {
            var model = new CampaignLandingViewModel();

            model.Introduction = new HtmlString(content.GetPropertyValue<string>("Introduction_Content"));

            model.LandingNavigation.Sections = BuildLandingLinksViewModelFromUmbracoContent(content, relatedLinksService);

            var buttonLinks = relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(content, "ButtonNavigation_Content");

            for (var i = 0; i <= 2 && i < buttonLinks.Count; i++)
            {
                var target = buttonLinks[i];
                if (target != null)
                {
                    model.ButtonTargets.Add(target);
                    model.ButtonDescriptions.Add(content.GetPropertyValue<string>("Button" + (i+1) + "Description_Content"));
                }
            }
            
            model.Content = new HtmlString(content.GetPropertyValue<string>("Content_Content"));

            var imageData = content.GetPropertyValue<IPublishedContent>("BackgroundSmall_Design");
            if (imageData != null)
            {
                model.BackgroundImageSmall = new Image()
                {
                    ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            imageData = content.GetPropertyValue<IPublishedContent>("BackgroundMedium_Design");
            if (imageData != null)
            {
                model.BackgroundImageMedium = new Image()
                {
                    ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            imageData = content.GetPropertyValue<IPublishedContent>("BackgroundLarge_Design");
            if (imageData != null)
            {
                model.BackgroundImageLarge = new Image()
                {
                    ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative))
                };
            }
            model.CustomCssSmallScreen = new HtmlString(content.GetPropertyValue<string>("CssSmall_Design"));
            model.CustomCssMediumScreen = new HtmlString(content.GetPropertyValue<string>("CssMedium_Design"));
            model.CustomCssLargeScreen = new HtmlString(content.GetPropertyValue<string>("CssLarge_Design"));
            
            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService, UmbracoContext.Current.InPreviewMode);

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
            var sections = new List<LandingSectionViewModel>() {};
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