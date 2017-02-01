using System;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Services;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.HomePage
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

            var viewModel = new HomePageItemViewModelFromUmbraco(model.Content).BuildModel();

            return CurrentTemplate(viewModel);
        }
    }
}