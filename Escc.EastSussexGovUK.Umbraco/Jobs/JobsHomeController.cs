using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Jobs' Umbraco document type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class JobsHomeController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = new JobsHomeViewModelFromUmbraco(model.Content,
                new RelatedLinksService(mediaUrlTransformer, new ElibraryUrlTransformer(), new RemoveAzureDomainUrlTransformer())).BuildModel();

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }
    }
}