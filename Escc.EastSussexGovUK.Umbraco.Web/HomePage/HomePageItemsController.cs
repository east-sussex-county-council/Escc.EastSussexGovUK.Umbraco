using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.Umbraco.Caching;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.Umbraco.Expiry;
using Examine;

namespace Escc.EastSussexGovUK.Umbraco.Web.HomePage
{
    /// <summary>
    /// The controller for the Home Page document type
    /// </summary>
    public class HomePageItemsController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = MapUmbracoContentToViewModel(model.Content);

            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new IExpiryDateSource[] { expiryDate, new ExpiryDateFromPropertyValue(model.Content, "latestUnpublishDate_Latest") });

            return CurrentTemplate(viewModel);
        }

        private static RssViewModel<HomePageItemViewModel> MapUmbracoContentToViewModel(IPublishedContent publishedContent)
        {
            var model = new RssViewModel<HomePageItemViewModel>();
            model.Metadata.Title = publishedContent.Name;
            model.Metadata.Description = publishedContent.GetPropertyValue<string>("pageDescription_Content");

            CampaignTrackingUrlTransformer linkUrlTransformer = null;
            var source = publishedContent.GetPropertyValue<string>("CampaignTrackingSource_Analytics");
            var medium = publishedContent.GetPropertyValue<string>("CampaignTrackingMedium_Analytics");
            var content = publishedContent.GetPropertyValue<string>("CampaignTrackingContent_Analytics");
            var campaign = publishedContent.GetPropertyValue<string>("CampaignTrackingCampaign_Analytics");
            var regex = publishedContent.GetPropertyValue<string>("CampaignTrackingRegex_Analytics");
            if (!String.IsNullOrEmpty(source) && !String.IsNullOrEmpty(medium) && !String.IsNullOrEmpty(campaign))
            {
                linkUrlTransformer = new CampaignTrackingUrlTransformer(source, medium, campaign, content, regex);
            }
            
            ((List<HomePageItemViewModel>)model.Items).AddRange(
                publishedContent.Children<IPublishedContent>()
                .Where(child => child.ContentType.Alias == "HomePageItem")
                .Select(child => new HomePageItemViewModelFromUmbraco(child, linkUrlTransformer).BuildModel())
                );
            return model;
        }
    }
}