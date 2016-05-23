using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.Caching;
using Escc.Umbraco.PropertyTypes;
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
            var model = new RssViewModel()
            {
                PageTitle = publishedContent.Name,
                Description = publishedContent.GetPropertyValue<string>("pageDescription_Content")
            };
            ((List<HomePageItemViewModel>)model.Items).AddRange(
                publishedContent.Children<IPublishedContent>()
                .Where(child => child.ContentType.Alias == "HomePageItem")
                .Select(MapUmbracoContentToItemViewModel)
                );
            return model;
        }

        internal static HomePageItemViewModel MapUmbracoContentToItemViewModel(IPublishedContent content)
        {
            var model = new HomePageItemViewModel()
            {
                Id = content.Id.ToString(CultureInfo.InvariantCulture),
                Description = content.GetPropertyValue<string>("ItemDescription_Content"),
                PublishedDate = content.UpdateDate
            };

            var targetUrl = content.GetPropertyValue<string>("URL_Content");
            if (targetUrl != null)
            {
                model.Link = new HtmlLink()
                {
                    Url = new Uri(targetUrl, UriKind.RelativeOrAbsolute),
                    Text = content.Name
                };
            }

            // Photo
            var imageData = content.GetPropertyValue<IPublishedContent>("Image_Content");
            if (imageData != null)
            {
                model.Image = new Image()
                {
                    ImageUrl = ContentHelper.TransformUrl(new Uri(imageData.Url, UriKind.Relative)),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
            }

            return model;
        }
    }
}