using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.EastSussexGovUK.Umbraco.Skins;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.Umbraco.Expiry;
using Escc.Umbraco.PropertyTypes;
using Examine;
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
            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            viewModel.LeadingText = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(model.Content.GetPropertyValue<string>("LeadingText_Content")));
            viewModel.FormGuid = model.Content.GetPropertyValue<Guid>("Form_Content");

            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, null,
                new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]).ExpiryDate,
                UmbracoContext.Current.InPreviewMode, new SkinFromUmbraco());
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                new UmbracoLatestService(model.Content),
                null, null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null, null);

            return CurrentTemplate(viewModel);
        }
    }
}