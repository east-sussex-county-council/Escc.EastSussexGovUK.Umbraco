using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.Umbraco.PropertyTypes;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.CampaignTemplates
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
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = new CampaignLandingViewModelFromUmbraco(model.Content, new RelatedLinksService(mediaUrlTransformer, new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer()), mediaUrlTransformer).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }
    }
}