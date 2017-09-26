using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    /// Displays a guide as an aggregation of its steps, or returns 404 if there are no steps
    /// </summary>
    public class GuideController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = MapUmbracoContentToViewModel(model.Content);
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);

            if (!viewModel.Steps.Any())
            {
                throw new HttpException(404, "Not found");
            }

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private static GuideStepViewModel MapUmbracoContentToGuideStepViewModel(IPublishedContent content)
        {
            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = new GuideStepViewModelFromUmbraco(content,
                    new RelatedLinksService(mediaUrlTransformer, new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer()),
                    mediaUrlTransformer
                    ).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                new UmbracoLatestService(content),
                new UmbracoSocialMediaService(content),
                new UmbracoEastSussex1SpaceService(content),
                new UmbracoWebChatSettingsService(content, new UrlListReader()),
                new UmbracoEscisService(content));

            return viewModel;
        }

        private static GuideViewModel MapUmbracoContentToViewModel(IPublishedContent content)
        {
            var model = new GuideViewModel()
            {
                Steps = new List<GuideStepViewModel>(content.Children<IPublishedContent>()
                    .Where(child => child.ContentType.Alias == "GuideStep")
                    .Select(MapUmbracoContentToGuideStepViewModel))
            };

            return model;
        }
    }
}