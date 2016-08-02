using System;
using System.Globalization;
using AST.AzureBlobStorage.Helper;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.HomePage;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Creates a <see cref="HomePageItemViewModel"/> from an Umbraco content node
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IHomePageItemProvider" />
    public class HomePageItemFromUmbraco : IHomePageItemProvider
    {
        private readonly IPublishedContent _umbracoContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomePageItemFromUmbraco"/> class.
        /// </summary>
        /// <param name="umbracoUmbracoContent">An instance of Umbraco content using the <see cref="HomePageItemDocumentType"/> document type.</param>
        public HomePageItemFromUmbraco(IPublishedContent umbracoUmbracoContent)
        {
            if (umbracoUmbracoContent == null) throw new ArgumentNullException(nameof(umbracoUmbracoContent));
            _umbracoContent = umbracoUmbracoContent;
        }

        /// <summary>
        /// Gets the home page item.
        /// </summary>
        /// <returns></returns>
        public HomePageItemViewModel GetHomePageItem()
        {
            var model = new HomePageItemViewModel()
            {
                Id = _umbracoContent.Id.ToString(CultureInfo.InvariantCulture),
                Description = _umbracoContent.GetPropertyValue<string>("ItemDescription_Content"),
                PublishedDate = _umbracoContent.UpdateDate
            };

            var targetUrl = _umbracoContent.GetPropertyValue<string>("URL_Content");
            if (targetUrl != null)
            {
                model.Link = new HtmlLink()
                {
                    Url = new Uri(targetUrl, UriKind.RelativeOrAbsolute),
                    Text = _umbracoContent.Name
                };
            }

            // Photo
            var imageData = _umbracoContent.GetPropertyValue<IPublishedContent>("Image_Content");
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