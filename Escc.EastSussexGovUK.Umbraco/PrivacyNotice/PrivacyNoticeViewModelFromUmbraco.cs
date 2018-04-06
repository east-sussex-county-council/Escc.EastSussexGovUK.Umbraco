using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Elibrary;
using Umbraco.Core.Models;
using Umbraco.Web;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Services;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// Builds a <see cref="PrivacyNoticeViewModel"/> from an Umbraco node based on <see cref="PrivacyNoticeDocumentType"/>
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder&lt;T&gt;" />
    public class PrivacyNoticeViewModelFromUmbraco : IViewModelBuilder<PrivacyNoticeViewModel>
    {
        private readonly IPublishedContent _umbracoContent;
        private readonly IElibraryProxyLinkConverter _elibraryLinkConverter;
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Builds a <see cref="PrivacyNoticeViewModel"/> from Umbraco content
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco.</param>
        /// <param name="elibraryLinkConverter">The elibrary link converter.</param>
        /// <param name="mediaUrlTransformer">A service to update links to items in the media library</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public PrivacyNoticeViewModelFromUmbraco(IPublishedContent umbracoContent, IElibraryProxyLinkConverter elibraryLinkConverter, IMediaUrlTransformer mediaUrlTransformer)
        {
            _umbracoContent = umbracoContent;
            _elibraryLinkConverter = elibraryLinkConverter;
            _mediaUrlTransformer = mediaUrlTransformer;

            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));
            if (elibraryLinkConverter == null) throw new ArgumentNullException(nameof(elibraryLinkConverter));
            if (mediaUrlTransformer == null) throw new ArgumentNullException(nameof(mediaUrlTransformer));
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <returns></returns>
        public PrivacyNoticeViewModel BuildModel()
        {
            var model = new PrivacyNoticeViewModel
            {
                WhatIsCovered = GetHtmlFromUmbraco("WhatIsCovered_What"),
                WhatIsUsed = GetHtmlFromUmbraco("WhatIsUsed_What"),
                HowItIsUsed = GetHtmlFromUmbraco("Why_Why"),
                OutsideEEA = _umbracoContent.GetPropertyValue<bool>("OutsideEEA_Why"),
                AutomatedDecisionMaking = GetHtmlFromUmbraco("AutomatedDecisionMaking_Why"),
                LegalBasis = GetHtmlFromUmbraco("LegalBasis_Legal_basis"),
                HowLong = GetHtmlFromUmbraco("HowLong_How_long"),
                SharingNeedToKnow = GetHtmlFromUmbraco("NeedToKnow_Sharing"),
                SharingThirdParties = GetHtmlFromUmbraco("ThirdParty_Sharing"),
                Contact = GetHtmlFromUmbraco("Contact_Contact")
            };

            return model;
        }

        private IHtmlString GetHtmlFromUmbraco(string propertyAlias)
        {
            var html = _umbracoContent.GetPropertyValue<string>(propertyAlias);
            if (!String.IsNullOrEmpty(html))
            {
                html = _mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(html);
                html = _elibraryLinkConverter.ParseAndRewriteElibraryLinks(html);
            }
            return new HtmlString(html);
        }
    }
}