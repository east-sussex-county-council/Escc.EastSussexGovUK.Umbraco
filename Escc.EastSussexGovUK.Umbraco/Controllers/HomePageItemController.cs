using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    /// <summary>
    /// The controller for the Home Page document type
    /// </summary>
    public class HomePageItemController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            var mediaUrlTransformer = new AzureMediaUrlTransformer(GlobalHelper.GetCdnDomain(), GlobalHelper.GetDomainsToReplace());
            var viewModel = new HomePageItemViewModelFromUmbraco(model.Content, mediaUrlTransformer).BuildModel();

            return CurrentTemplate(viewModel);
        }
    }
}