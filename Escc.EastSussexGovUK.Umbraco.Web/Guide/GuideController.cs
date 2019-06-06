using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Skins;
using Examine;
using Escc.Umbraco.Expiry;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Mvc;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Web.Guide
{
    /// <summary>
    /// Displays a guide as an aggregation of its steps, or returns 404 if there are no steps
    /// </summary>
    public class GuideController : RenderMvcController
    {
        public new async Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));
            var templateRequest = new EastSussexGovUKTemplateRequest(Request, webChatSettingsService: new UmbracoWebChatSettingsService(model.Content, new UrlListReader()));
            var viewModel = await MapUmbracoContentToViewModel(model.Content, expiryDate.ExpiryDate, templateRequest);
            var modelBuilder = new BaseViewModelBuilder(templateRequest);
            await modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
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

        private async static Task<GuideStepViewModel> MapUmbracoContentToGuideStepViewModel(IPublishedContent content, IEastSussexGovUKTemplateRequest templateRequest)
        {
            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = new GuideStepViewModelFromUmbraco(content,
                    new RelatedLinksService(mediaUrlTransformer, new RemoveAzureDomainUrlTransformer()),
                    mediaUrlTransformer
                    ).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder(templateRequest);
            await modelBuilder.PopulateBaseViewModel(viewModel, content, new ContentExperimentSettingsService(),
                new ExpiryDateFromExamine(content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1))).ExpiryDate,
                UmbracoContext.Current.InPreviewMode, new SkinFromUmbraco());
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                new UmbracoLatestService(content),
                new UmbracoSocialMediaService(content),
                new UmbracoEastSussex1SpaceService(content),
                new UmbracoEscisService(content));

            return viewModel;
        }

        private async static Task<GuideViewModel> MapUmbracoContentToViewModel(IPublishedContent content, DateTime? expiryDate, IEastSussexGovUKTemplateRequest templateRequest)
        {
            var stepPages = content.Children<IPublishedContent>().Where(child => child.ContentType.Alias == "GuideStep");
            var steps = new List<GuideStepViewModel>();
            foreach (var step in stepPages)
            {
                steps.Add(await MapUmbracoContentToGuideStepViewModel(step, templateRequest));
            }
            var model = new GuideViewModel() { Steps = steps };

            var sectionNavigation = content.GetPropertyValue<int>("SectionNavigation_Navigation");
            model.StepsHaveAnOrder = (sectionNavigation == 0 || umbraco.library.GetPreValueAsString(sectionNavigation).ToUpperInvariant() != "BULLETED LIST");

            return model;
        }
    }
}