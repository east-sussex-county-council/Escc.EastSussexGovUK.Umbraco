using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Elibrary;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Ratings;
using Escc.EastSussexGovUK.Umbraco.Web.Skins;
using Examine;
using Escc.Umbraco.Expiry;
using Escc.EastSussexGovUK.Umbraco.Web.Latest;
using Escc.EastSussexGovUK.Mvc;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Web.PrivacyNotice
{
    /// <summary>
    /// Controller for displaying a page using the 'Privacy notice' Umbraco data type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class PrivacyNoticeController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public new async Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var mediaUrlTransformer = new RemoveMediaDomainUrlTransformer();
            var viewModel = new PrivacyNoticeViewModelFromUmbraco(model.Content,
                    new ElibraryProxyLinkConverter(new SpydusUrlBuilder()),
                    mediaUrlTransformer
                    ).BuildModel();

            // Add common properties to the model
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));
            var modelBuilder = new BaseViewModelBuilder(new EastSussexGovUKTemplateRequest(Request));
            await modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode, new SkinFromUmbraco());
            await modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                null,
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null,
                new RatingSettingsFromUmbraco(model.Content));

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") });

            return CurrentTemplate(viewModel);
        }
    }
}