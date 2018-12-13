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
using Escc.EastSussexGovUK.Umbraco.Skins;
using Examine;
using Escc.Umbraco.Expiry;
using Escc.EastSussexGovUK.Umbraco.Latest;

namespace Escc.EastSussexGovUK.Umbraco.Guide
{
    /// <summary>
    /// Displays a guide as an aggregation of its steps, or returns 404 if there are no steps
    /// </summary>
    public class GuideController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var viewModel = MapUmbracoContentToViewModel(model.Content, expiryDate.ExpiryDate);
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode, new SkinFromUmbraco());

            if (!viewModel.Steps.Any())
            {
                return new HttpNotFoundResult();
            }

            // This only has one view, for printing, so if not requested as such, redirect
            if (Request.Url != null && !Request.Url.AbsolutePath.EndsWith("/print", StringComparison.OrdinalIgnoreCase))
            {
                var firstStep = viewModel.Steps.First();
                if (firstStep != null && firstStep.Steps.Any())
                {
                    Response.Headers.Add("Location", new Uri(Request.Url, firstStep.Steps.First().Url).ToString());
                    return new HttpStatusCodeResult(303);
                }
            }

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") });

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
            modelBuilder.PopulateBaseViewModel(viewModel, content, new ContentExperimentSettingsService(),
                new ExpiryDateFromExamine(content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]).ExpiryDate,
                UmbracoContext.Current.InPreviewMode, new SkinFromUmbraco());
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                new UmbracoLatestService(content),
                new UmbracoSocialMediaService(content),
                new UmbracoEastSussex1SpaceService(content),
                new UmbracoWebChatSettingsService(content, new UrlListReader()),
                new UmbracoEscisService(content));

            return viewModel;
        }

        private static GuideViewModel MapUmbracoContentToViewModel(IPublishedContent content, DateTime? expiryDate)
        {
            var model = new GuideViewModel()
            {
                Steps = new List<GuideStepViewModel>(content.Children<IPublishedContent>()
                    .Where(child => child.ContentType.Alias == "GuideStep")
                    .Select(MapUmbracoContentToGuideStepViewModel))
            };

            var sectionNavigation = content.GetPropertyValue<int>("SectionNavigation_Navigation");
            model.StepsHaveAnOrder = (sectionNavigation == 0 || umbraco.library.GetPreValueAsString(sectionNavigation).ToUpperInvariant() != "BULLETED LIST");

            return model;
        }
    }
}