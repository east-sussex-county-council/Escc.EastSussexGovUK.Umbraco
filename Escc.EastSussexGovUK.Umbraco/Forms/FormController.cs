using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.EastSussexGovUK.Umbraco.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Forms
{
    /// <summary>
    /// Controller for an Umbraco content page hosting an Umbraco form
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class FormController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {
            var viewModel = new FormModel();

            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, null, UmbracoContext.Current.InPreviewMode, new SkinFromUmbraco());

            return CurrentTemplate(viewModel);
        }
    }
}