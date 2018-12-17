using System;
using System.Collections.Generic;
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
using Escc.EastSussexGovUK.Umbraco.Web.Ratings;
using Escc.EastSussexGovUK.Umbraco.Web.Skins;
using Examine;
using Escc.Umbraco.Expiry;
using latest = Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration
{
    public class MicrosoftCmsPageController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var landingModel = MapUmbracoContentToViewModel(model.Content, expiryDate.ExpiryDate,
                new UmbracoLatestService(model.Content),
                new UmbracoSocialMediaService(model.Content),
                new UmbracoEastSussex1SpaceService(model.Content), 
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()), 
                new ContentExperimentSettingsService(),
                new UmbracoEscisService(model.Content),
                new RatingSettingsFromUmbraco(model.Content),
                new SkinFromUmbraco());

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") });

            return CurrentTemplate(landingModel);
        }

        private static MicrosoftCmsViewModel MapUmbracoContentToViewModel(IPublishedContent content, DateTime? expiryDate, latest.ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService, IRatingSettingsProvider ratingSettings, ISkinToApplyService skinService)
        {
            var model = new MicrosoftCmsViewModel();

            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService,
                expiryDate,
                UmbracoContext.Current.InPreviewMode, skinService);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(model, latestService, socialMediaService, eastSussex1SpaceService, webChatSettingsService, escisService, ratingSettings);

            return model;
        }
    }
}