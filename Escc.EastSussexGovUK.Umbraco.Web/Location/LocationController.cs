using System;
using System.Web.Mvc;
using Escc.Dates;
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
using Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Mvc;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Web.Location
{
    public class LocationController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public new async Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));
            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = new LocationViewModelFromUmbraco(model.Content, 
                    mediaUrlTransformer,
                    new RelatedLinksService(mediaUrlTransformer, new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer())
                    ).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder(new EastSussexGovUKTemplateRequest(Request, webChatSettingsService: new UmbracoWebChatSettingsService(model.Content, new UrlListReader())));
            await modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode, new SkinFromUmbraco());
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content),
                new UmbracoSocialMediaService(model.Content),
                new UmbracoEastSussex1SpaceService(model.Content), 
                new UmbracoEscisService(model.Content),
                new RatingSettingsFromUmbraco(model.Content));

            viewModel = UpdateLocationViewModel(model.Content, viewModel);

            SetupHttpCaching(model, viewModel, expiryDate);

            return CurrentTemplate(viewModel);
        }

        /// <summary>
        /// Provide a way for child classes to update the view model based on Umbraco content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        protected virtual LocationViewModel UpdateLocationViewModel(IPublishedContent content, LocationViewModel viewModel)
        {
            return viewModel;
        }

        private void SetupHttpCaching(RenderModel model, LocationViewModel viewModel, IExpiryDateSource expiryDate)
        {
            var cacheService = new HttpCachingService();
            var cacheExpiryProperties = new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") };
            var ukNow = DateTime.Now.ToUkDateTime();
            const int oneHour = 3600;

            // Ensure we don't cache the page longer than the opening times data passed to the view remains valid.
            // In the last hour before closing there's a countdown, so it's only correct for one minute.
            // If there's no opening times data, just fall back to the default cache expiry settings.
            if (viewModel.OpenUntil.HasValue)
            {
                var secondsRemaining = Convert.ToInt32((viewModel.OpenUntil - ukNow).Value.TotalSeconds);
                if (secondsRemaining <= oneHour) secondsRemaining = 60;
                cacheService.SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, cacheExpiryProperties, secondsRemaining);
            }
            else if (viewModel.NextOpen.HasValue)
            {
                var secondsRemaining = Convert.ToInt32((viewModel.NextOpen - ukNow).Value.TotalSeconds);
                if (secondsRemaining <= oneHour) secondsRemaining = 60;
                cacheService.SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, cacheExpiryProperties, secondsRemaining);
            }
            else
            {
                cacheService.SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, cacheExpiryProperties);
            }
        }

    }
}