using System;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.Umbraco.Caching;
using Examine;
using Escc.Umbraco.Expiry;
using Escc.EastSussexGovUK.Umbraco.Examine;
using System.Threading.Tasks;
using System.Configuration;

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayModifications
{
    /// <summary>
    /// Controller for pages based on the 'Rights of way Section 31 definitive map modifications RSS feed' Umbraco document type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class RightsOfWayModificationsRssController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public new async Task<ActionResult> Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (ExamineManager.Instance.SearchProviderCollection["RightsOfWayModificationsSearcher"] == null)
            {
                throw new ConfigurationErrorsException("The RightsOfWayModificationsSearcher is not configured in Examine config");
            }

            var viewModel = new RightsOfWayModificationsViewModelFromExamine(model.Content.Parent.Id, new Uri(model.Content.Parent.UrlAbsolute()), ExamineManager.Instance.SearchProviderCollection["RightsOfWayModificationsSearcher"], Request.QueryString["q"], new ISearchFilter[] { new SearchTermSanitiser() }, Umbraco).BuildModel();

            var rss = new RssViewModel<RightsOfWayModificationViewModel>();
            foreach (var modificationOrder in viewModel.ModificationOrderApplications) rss.Items.Add(modificationOrder);

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"], new ExpiryDateMemoryCache(TimeSpan.FromHours(1)));
            var modelBuilder = new BaseViewModelBuilder(null);
            await modelBuilder.PopulateBaseViewModel(rss, model.Content, null,
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);

            if (!String.IsNullOrEmpty(Request.QueryString["q"]))
            {
                rss.Metadata.Title += $" matching '{Request.QueryString["q"]}'";
            }

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new[] { expiryDate });

            return CurrentTemplate(rss);
        }
    }
}