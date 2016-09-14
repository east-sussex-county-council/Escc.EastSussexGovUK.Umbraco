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
    /// <seealso cref="IViewModelBuilder&lt;T&gt;" />
    public class HomePageItemViewModelFromUmbraco : IViewModelBuilder<HomePageItemViewModel>
    {
        private readonly IPublishedContent _umbracoContent;
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomePageItemViewModelFromUmbraco"/> class.
        /// </summary>
        /// <param name="umbracoContent">An instance of Umbraco content using the <see cref="HomePageItemDocumentType"/> document type.</param>
        /// <param name="mediaUrlTransformer">A service to links to items in the media library</param>
        public HomePageItemViewModelFromUmbraco(IPublishedContent umbracoContent, IMediaUrlTransformer mediaUrlTransformer)
        {
            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));
            if (mediaUrlTransformer == null) throw new ArgumentNullException(nameof(mediaUrlTransformer));

            _umbracoContent = umbracoContent;
            _mediaUrlTransformer = mediaUrlTransformer;
        }

        /// <summary>
        /// Gets the home page item.
        /// </summary>
        /// <returns></returns>
        public HomePageItemViewModel BuildModel()
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
                    ImageUrl = _mediaUrlTransformer.TransformMediaUrl(new Uri(imageData.Url, UriKind.Relative)),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
            }

            return model;
        }
    }
}