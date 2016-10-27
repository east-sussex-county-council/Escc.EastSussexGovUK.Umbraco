using System;
using System.Collections.Generic;
using System.Linq;
using Escc.EastSussexGovUK.Features;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Reads settings for Web Chat from fields on an <see cref="IPublishedContent"/> using the <c>Web chat</c> document type from <c>Escc.EastSussexGovUK.UmbracoDocumentTypes</c>.
    /// </summary>
    public class UmbracoWebChatSettingsService : IWebChatSettingsService
    {
        private IPublishedContent _content;
        private readonly IUrlListReader _targetUrlReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoWebChatSettingsService"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="targetUrlReader">The URL list reader.</param>
        public UmbracoWebChatSettingsService(IPublishedContent content, IUrlListReader targetUrlReader)
        {
            if (content==null) throw new ArgumentNullException("content");
            if (targetUrlReader==null) throw new ArgumentNullException("targetUrlReader");

            _content = content;
            _targetUrlReader = targetUrlReader;
        }

        /// <summary>
        /// Reads the web chat settings
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">content</exception>
        public WebChatSettings ReadWebChatSettings()
        {
            if (_content == null) throw new ArgumentNullException("content");

            var model = new WebChatSettings
            {
                PageUrl = new Uri(_content.Url, UriKind.RelativeOrAbsolute)
            };

            var webChatSettings = _content.AncestorOrSelf(1).Siblings().FirstOrDefault(sibling => sibling.DocumentTypeAlias == "WebChat");
            if (webChatSettings == null) return model;

            ((List<Uri>) model.WebChatUrls).AddRange(_targetUrlReader.ReadUrls(webChatSettings, "whereToDisplayIt_Content", "whereElseToDisplayIt_Content"));
            ((List<Uri>) model.ExcludedUrls).AddRange(_targetUrlReader.ReadUrls(webChatSettings, "whereToExclude_Content", "whereElseToExclude_Content"));
            return model;
        }
    }
}