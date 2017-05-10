using System;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Controller for pages based on the 'Jobs component' Umbraco document type
    /// </summary>
    public class JobsComponentController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new ComponentViewModelFromUmbraco(model.Content, new RemoveMediaDomainUrlTransformer()).BuildModel();

            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                  new UmbracoLatestService(model.Content),
                  new UmbracoSocialMediaService(model.Content),
                  null,
                  new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                  null);

            return CurrentTemplate(viewModel);
        }
    }
}