using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    /// <summary>
    /// Displays a guide as an aggregation of its steps, or returns 404 if there are no steps
    /// </summary>
    public class GuideController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var viewModel = MapUmbracoContentToViewModel(model.Content);

            if (!viewModel.Steps.Any())
            {
                throw new HttpException(404, "Not found");
            }

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private static GuideStepViewModel MapUmbracoContentToGuideStepViewModel(IPublishedContent content)
        {
            return GuideStepController.MapUmbracoContentToViewModel(content,
                new UmbracoLatestService(content),
                new UmbracoSocialMediaService(content),
                new UmbracoEastSussex1SpaceService(content),
                new UmbracoWebChatSettingsService(content, new UrlListReader()),
                new UmbracoOnAzureRelatedLinksService(new AzureMediaUrlTransformer(GlobalHelper.GetCdnDomain(), GlobalHelper.GetDomainsToReplace())),
                new ContentExperimentSettingsService(),
                new UmbracoEscisService(content));
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