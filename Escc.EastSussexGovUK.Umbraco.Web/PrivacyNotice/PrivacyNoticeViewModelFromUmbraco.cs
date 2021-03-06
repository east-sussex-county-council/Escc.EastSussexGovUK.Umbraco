﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Web.Services;

namespace Escc.EastSussexGovUK.Umbraco.Web.PrivacyNotice
{
    /// <summary>
    /// Builds a <see cref="PrivacyNoticeViewModel"/> from an Umbraco node based on <see cref="PrivacyNoticeDocumentType"/>
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder&lt;T&gt;" />
    public class PrivacyNoticeViewModelFromUmbraco : IViewModelBuilder<PrivacyNoticeViewModel>
    {
        private readonly IPublishedContent _umbracoContent;
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        /// <summary>
        /// Builds a <see cref="PrivacyNoticeViewModel"/> from Umbraco content
        /// </summary>
        /// <param name="umbracoContent">Content from Umbraco.</param>
        /// <param name="mediaUrlTransformer">A service to update links to items in the media library</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public PrivacyNoticeViewModelFromUmbraco(IPublishedContent umbracoContent, IMediaUrlTransformer mediaUrlTransformer)
        {
            _umbracoContent = umbracoContent;
            _mediaUrlTransformer = mediaUrlTransformer;

            if (umbracoContent == null) throw new ArgumentNullException(nameof(umbracoContent));
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
                WhatIsCovered = GetHtmlFromUmbraco("WhatIsCovered_Overview"),
                WhatIsUsed = GetHtmlFromUmbraco("WhatIsUsed_What_is_used"),
                HowItIsUsed = GetHtmlFromUmbraco("HowUsed_How_is_it_used"),
                OutsideEEA = _umbracoContent.GetPropertyValue<bool>("OutsideEEA_How_is_it_used"),
                AutomatedDecisionMaking = GetHtmlFromUmbraco("AutomatedDecisionMaking_How_is_it_used"),
                LegalBasis = GetHtmlFromUmbraco("LegalBasis_Legal_basis"),
                HowLong = GetHtmlFromUmbraco("HowLong_How_long_for"),
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
            }
            return new HtmlString(html);
        }
    }
}