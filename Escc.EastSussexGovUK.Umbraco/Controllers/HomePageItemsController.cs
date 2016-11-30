using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.Caching;
using Escc.Umbraco.PropertyTypes;
using Escc.Web.Metadata;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
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
            if (model == null) throw new ArgumentNullException("model");

            var viewModel = MapUmbracoContentToViewModel(model.Content);

            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new List<string>() { "latestUnpublishDate_Latest" });

            return CurrentTemplate(viewModel);
        }

        private static RssViewModel MapUmbracoContentToViewModel(IPublishedContent publishedContent)
        {
            var model = new RssViewModel();
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
            
            var mediaUrlTransformer = new AzureMediaUrlTransformer(GlobalHelper.GetCdnDomain(), GlobalHelper.GetDomainsToReplace());

            ((List<HomePageItemViewModel>)model.Items).AddRange(
                publishedContent.Children<IPublishedContent>()
                .Where(child => child.ContentType.Alias == "HomePageItem")
                .Select(child => new HomePageItemViewModelFromUmbraco(child, mediaUrlTransformer, linkUrlTransformer).BuildModel())
                );
            return model;
        }
    }
}