using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Ratings;
using Escc.EastSussexGovUK.Umbraco.Web.Skins;
using Examine;
using Escc.Umbraco.Expiry;
using latest = Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;

namespace Escc.EastSussexGovUK.Umbraco.Web.Landing
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

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));
            var viewModel = MapUmbracoContentToViewModel(model.Content, expiryDate.ExpiryDate,
                new UmbracoLatestService(model.Content),
                new UmbracoSocialMediaService(model.Content), 
                new UmbracoEastSussex1SpaceService(model.Content), 
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()), 
                new RelatedLinksService(new RemoveMediaDomainUrlTransformer(), new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer()), 
                new ContentExperimentSettingsService(), 
                new UmbracoEscisService(model.Content),
                new RatingSettingsFromUmbraco(model.Content), 
                new SkinFromUmbraco());

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") });

            return CurrentTemplate(viewModel);
        }

        private static LandingViewModel MapUmbracoContentToViewModel(IPublishedContent content, DateTime? expiryDate, latest.ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IRelatedLinksService relatedLinksService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService, IRatingSettingsProvider ratingSettings, ISkinToApplyService skinService)
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
            modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService,
                expiryDate,
                UmbracoContext.Current.InPreviewMode, skinService);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(model, latestService, socialMediaService, eastSussex1SpaceService, webChatSettingsService, escisService, ratingSettings);

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