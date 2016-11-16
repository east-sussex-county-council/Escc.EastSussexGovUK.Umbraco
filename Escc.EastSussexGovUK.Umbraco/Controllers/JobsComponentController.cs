using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.ContentExperiments;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    /// <summary>
    /// Controller for pages based on the 'Jobs component' Umbraco document type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class JobsComponentController : RenderMvcController
    {
        // GET: TalentLinkComponent
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new JobsComponentViewModel();
            viewModel.ScriptUrl = new Uri(model.Content.GetPropertyValue<string>("ScriptUrl_Content"));
            viewModel.LinkUrl = new Uri(viewModel.ScriptUrl.ToString().Replace("laydisplayrapido.cfm", "jsoutputinitrapido.cfm").Replace("&browserchk=no", String.Empty));

            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            viewModel.Metadata.Description = String.Empty;


            return CurrentTemplate(viewModel);
        }
    }
}