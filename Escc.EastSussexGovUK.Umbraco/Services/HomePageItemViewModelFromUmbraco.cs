using System;
using System.Globalization;
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
        private readonly IUrlTransformer _linkUrlTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomePageItemViewModelFromUmbraco" /> class.
        /// </summary>
        /// <param name="umbracoContent">An instance of Umbraco content using the <see cref="HomePageItemDocumentType" /> document type.</param>
        /// <param name="linkUrlTransformer">The link URL transformer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// umbracoContent
        /// or
        /// mediaUrlTransformer
        /// </exception>
        public HomePageItemViewModelFromUmbraco(IPublishedContent umbracoContent, IUrlTransformer linkUrlTransformer=null)
        {
            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));

            _umbracoContent = umbracoContent;
            _linkUrlTransformer = linkUrlTransformer;
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
                if (_linkUrlTransformer != null)
                {
                    model.Link.Url = _linkUrlTransformer.TransformUrl(model.Link.Url);
                }
            }

            // Photo
            var imageData = _umbracoContent.GetPropertyValue<IPublishedContent>("Image_Content");
            if (imageData != null)
            {
                model.Image = new Image()
                {
                    ImageUrl = new Uri(imageData.Url, UriKind.Relative),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
            }

            return model;
        }
    }
}