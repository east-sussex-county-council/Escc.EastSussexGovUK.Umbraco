using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Elibrary;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Ratings;
using Escc.EastSussexGovUK.Umbraco.Skins;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    public class TaskController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = new TaskViewModelFromUmbraco(model.Content,
                    new RelatedLinksService(mediaUrlTransformer, new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer()),
                    new ElibraryProxyLinkConverter(new SpydusUrlBuilder()),
                    mediaUrlTransformer
                    ).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode, new SkinFromUmbraco());
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                new UmbracoEastSussex1SpaceService(model.Content),
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                new UmbracoEscisService(model.Content),
                new RatingSettingsFromUmbraco(model.Content));

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }
    }
}