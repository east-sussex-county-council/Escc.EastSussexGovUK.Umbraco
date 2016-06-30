using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Escc.EastSussexGovUK.MasterPages.Features;
using Escc.EastSussexGovUK.UmbracoViews.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using EsccWebTeam.EastSussexGovUK;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration
{
    public class MicrosoftCmsPageController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            var landingModel = MapUmbracoContentToViewModel(model.Content,
                new UmbracoLatestService(model.Content),
                new UmbracoSocialMediaService(model.Content, new EastSussexGovUKContext().DoNotTrack),
                new UmbracoEastSussex1SpaceService(model.Content), 
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()), 
                new ContentExperimentSettingsService(),
                new UmbracoEscisService(model.Content));

            return CurrentTemplate(landingModel);
        }

        private static MicrosoftCmsViewModel MapUmbracoContentToViewModel(IPublishedContent content, ILatestService latestService, ISocialMediaService socialMediaService, IEastSussex1SpaceService eastSussex1SpaceService, IWebChatSettingsService webChatSettingsService, IContentExperimentSettingsService contentExperimentSettingsService, IEscisService escisService)
        {
            var model = new MicrosoftCmsViewModel();

            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(model, content, contentExperimentSettingsService, UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(model, latestService, socialMediaService, eastSussex1SpaceService, webChatSettingsService, escisService);

            return model;
        }
    }
}