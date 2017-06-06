using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Examine;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.NavigationControls.WebForms;
using Escc.EastSussexGovUK.Umbraco.Examine;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Controller for pages based on the 'Rights of way Section 31 deposits' Umbraco document type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class RightsOfWayDepositsController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var paging = new PagingController()
            {
                ResultsTextSingular = "deposit",
                ResultsTextPlural = "deposits",
                PageSize = 5
            };

            var query = HttpUtility.ParseQueryString(Request.Url.Query);
            RightsOfWayDepositsSortOrder sort = RightsOfWayDepositsSortOrder.DateDepositedDescending;
            Enum.TryParse(query["sort"], true, out sort);

            var viewModel = new RightsOfWayDepositsViewModelFromExamine(model.Content.Id, Request.Url, Request.QueryString["q"], new ISearchFilter[] { new SearchTermSanitiser() }, paging.CurrentPage, paging.PageSize, sort).BuildModel();
            viewModel.Paging = paging;
            viewModel.Paging.TotalResults = viewModel.TotalDeposits;
            viewModel.SortOrder = sort;

            // Add common properties to the model
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(), UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);

//            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache);

            return CurrentTemplate(viewModel);
        }
    }
}