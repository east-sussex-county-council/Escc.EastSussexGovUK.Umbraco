using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    /// <summary>
    /// Controller used by the "Landing" document type
    /// </summary>
    public class LandingController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var viewModel = MapUmbracoContentToViewModel(model.Content, 
                new UmbracoLatestService(model.Content),
                new UmbracoSocialMediaService(model.Content), 
                new UmbracoEastSussex1SpaceService(model.Content), 
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()), 
                new RelatedLinksService(new RemoveMediaDomainUrlTransformer(), new ElibraryUrlTransformer()), 
                new ContentExperimentSettingsService(), 
                new UmbracoEscisService(model.Content));

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private static LandingViewModel MapUmbracoContentToViewModel(IPublishedContent content, ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IRelatedLinksService relatedLinksService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService)
        {
            var model = new LandingViewModel();
            model.Navigation.Sections = BuildLandingLinksViewModelFromUmbracoContent(content, relatedLinksService);
            model.Navigation.LandingNavigationLayout = LandingNavigationLayout.ThreeColumn;
            var selectedLayout = content.GetPropertyValue<string>("layout_Content");
            if (!String.IsNullOrEmpty(selectedLayout))
            {
                var selectedOption =
                    umbraco.library.GetPreValueAsString(Int32.Parse(selectedLayout, CultureInfo.InvariantCulture));

                if (selectedOption.ToUpperInvariant().Contains("TWO")) model.Navigation.LandingNavigationLayout = LandingNavigationLayout.TwoColumn;
            }

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService, UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(model, latestService, socialMediaService, eastSussex1SpaceService, webChatSettingsService, escisService);

            return model;
        }

        /// <summary>
        /// For each Umbraco Related Links property that has some data, create a <see cref="LandingSectionViewModel" /> with a heading link and list of child links.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="relatedLinksService">The related links service.</param>
        /// <returns></returns>
        private static IList<LandingSectionViewModel> BuildLandingLinksViewModelFromUmbracoContent(IPublishedContent content, IRelatedLinksService relatedLinksService)
        {
            var sections = new List<LandingSectionViewModel>();
            for (var i = 1; i <= 15; i++)
            {
                var relatedLinks = relatedLinksService.BuildRelatedLinksViewModelFromUmbracoContent(content, "links" + i + "_Content");
                if (relatedLinks.Count > 0)
                {
                    var section = new LandingSectionViewModel()
                    {
                        Links = relatedLinks
                    };

                    section.Heading = section.Links[0];
                    section.Links.RemoveAt(0);

                    sections.Add(section);
                }
            }
            return sections;
        }
    }
}