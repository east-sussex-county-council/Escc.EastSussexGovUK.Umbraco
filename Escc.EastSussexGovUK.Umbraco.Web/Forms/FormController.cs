using Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.EastSussexGovUK.Umbraco.Web.Skins;
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
using Escc.EastSussexGovUK.Mvc;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Web.Forms
{
    /// <summary>
    /// Controller for an Umbraco content page hosting an Umbraco form
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class FormController : RenderMvcController
    {
        public new async Task<ActionResult> Index(RenderModel model)
        {
            var viewModel = new FormModel();
            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            viewModel.LeadingText = new HtmlString(mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(model.Content.GetPropertyValue<string>("LeadingText_Content")));
            viewModel.FormGuid = model.Content.GetPropertyValue<Guid>("Form_Content");

            var modelBuilder = new BaseViewModelBuilder(new EastSussexGovUKTemplateRequest(Request));
            await modelBuilder.PopulateBaseViewModel(viewModel, model.Content, null,
                new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1))).ExpiryDate,
                UmbracoContext.Current.InPreviewMode, new SkinFromUmbraco());
            await modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel,
                new UmbracoLatestService(model.Content),
                null, null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null, null);

            return CurrentTemplate(viewModel);
        }
    }
}