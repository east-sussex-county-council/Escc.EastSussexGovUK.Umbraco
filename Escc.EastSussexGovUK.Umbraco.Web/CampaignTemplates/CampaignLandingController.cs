using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.Umbraco.PropertyTypes;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Examine;
using Escc.Umbraco.Expiry;
using Escc.EastSussexGovUK.Mvc;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Web.CampaignTemplates
{
    /// <summary>
    /// A landing page for marketing campaigns
    /// </summary>
    public class CampaignLandingController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public new async Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = new CampaignLandingViewModelFromUmbraco(model.Content, new RelatedLinksService(mediaUrlTransformer, new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer()), mediaUrlTransformer).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder(new EastSussexGovUKTemplateRequest(Request));
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));
            await modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource [] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") });

            return CurrentTemplate(viewModel);
        }
    }
}