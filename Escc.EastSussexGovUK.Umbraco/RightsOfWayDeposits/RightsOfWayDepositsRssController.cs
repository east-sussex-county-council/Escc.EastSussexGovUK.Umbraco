using System;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.Caching;
using Examine;
using Escc.Umbraco.Expiry;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Controller for pages based on the 'Rights of way Section 31 deposits RSS feed' Umbraco document type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class RightsOfWayDepositsRssController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new RightsOfWayDepositsViewModelFromExamine(model.Content.Parent.Id, new Uri(model.Content.Parent.UrlAbsolute()), Request.QueryString["q"], new ISearchFilter[] { new SearchTermSanitiser() }, Umbraco).BuildModel();

            var rss = new RssViewModel<RightsOfWayDepositViewModel>();
            foreach (var deposit in viewModel.Deposits) rss.Items.Add(deposit);

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(rss, model.Content, null,
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