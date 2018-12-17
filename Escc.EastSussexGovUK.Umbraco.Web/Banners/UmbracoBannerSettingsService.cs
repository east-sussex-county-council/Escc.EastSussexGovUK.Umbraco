using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Banners
{
    /// <summary>
    /// Reads settings for promotion banners from Umbraco pages
    /// </summary>
    public class UmbracoBannerSettingsService : IBannerSettingsService
    {
        private readonly IPublishedContent _content;
        private readonly IUrlListReader _targetUrlReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoBannerSettingsService" /> class.
        /// </summary>
        /// <param name="content">An Umbraco page using the <see cref="BannersDocumentType"/> document type.</param>
        /// <param name="targetUrlReader">The URL list reader.</param>
        /// <exception cref="System.ArgumentNullException">
        /// content
        /// or
        /// targetUrlReader
        /// </exception>
        public UmbracoBannerSettingsService(IPublishedContent content, IUrlListReader targetUrlReader)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (targetUrlReader == null) throw new ArgumentNullException("targetUrlReader");
            _content = content;
            _targetUrlReader = targetUrlReader;
        }

        /// <summary>
        /// Reads the banner settings from Umbraco pages using the <see cref="BannerDocumentType"/> document type.
        /// </summary>
        /// <returns></returns>
        public IList<Banner> ReadBannerSettings()
        {
            var model = new List<Banner>();

            foreach (var bannerPage in _content.Children)
            {
                var banner = ReadBannerFromUmbraco(bannerPage, _targetUrlReader);
                if (banner != null)
                {
                    model.Add(banner);
                }
            }

            return model;
        }

        /// <summary>
        /// Get banner settings from an Umbraco page using the <see cref="BannerDocumentType"/> document type
        /// </summary>
        /// <param name="bannerPage">The bannerPage.</param>
        /// <param name="urlListReader">The URL list reader.</param>
        private static Banner ReadBannerFromUmbraco(IPublishedContent bannerPage, IUrlListReader urlListReader)
        {
            var model = new Banner()
            {
                Inherit = bannerPage.GetPropertyValue<bool>("inherit_Content"),
                Cascade = bannerPage.GetPropertyValue<bool>("cascade_Content")
            };

            // Minimum requirement is just somewhere to display it. Return null if missing.
            // An image is not required because it's useful to set up a banner with no image as a way of stopping other banners.
            var imageData = bannerPage.GetPropertyValue<IPublishedContent>("bannerImage_Content");
            if (imageData != null)
            {
                model.BannerImage = new Image()
                {
                    AlternativeText = imageData.Name,
                    ImageUrl = new Uri(imageData.Url, UriKind.Relative),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
            }
            ((List<Uri>)model.TargetUrls).AddRange(urlListReader.ReadUrls(bannerPage, "whereToDisplayIt_Content", "whereElseToDisplayIt_Content"));
            if (model.TargetUrls.Count == 0) return null;

            // URL to link to is optional, and can come from two possible fields
            var targetPage = bannerPage.GetPropertyValue<IPublishedContent>("targetPage_Content");
            if (targetPage != null)
            {
                model.BannerLink = new Uri(targetPage.UrlWithDomain());
            }
            else
            {
                try
                {
                    var urlString = bannerPage.GetPropertyValue<string>("targetUrl_Content");
                    if (!String.IsNullOrEmpty(urlString))
                    {
                        model.BannerLink = new Uri(urlString, UriKind.RelativeOrAbsolute);
                    }
                }
                catch (UriFormatException) { 
                    // ignore invalid URLs
                }
            }

            return model;
        }
    }
}