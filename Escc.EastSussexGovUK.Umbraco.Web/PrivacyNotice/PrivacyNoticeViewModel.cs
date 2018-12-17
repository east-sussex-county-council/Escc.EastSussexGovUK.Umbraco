using Escc.EastSussexGovUK.Umbraco.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.PrivacyNotice
{
    /// <summary>
    /// Data to be displayed on a page using the 'Privacy notice' document type
    /// </summary>
    public class PrivacyNoticeViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets what this privacy notice covers.
        /// </summary>
        public IHtmlString WhatIsCovered { get; set; }
        
        /// <summary>
        /// Gets or sets what information is used.
        /// </summary>
        public IHtmlString WhatIsUsed { get; set; }

        /// <summary>
        /// Gets or sets how information is used.
        /// </summary>
        public IHtmlString HowItIsUsed { get; set; }

        /// <summary>
        /// Gets or sets whether information is processed outside the EEA
        /// </summary>
        /// <value>
        ///   <c>true</c> if outside the EEA; otherwise, <c>false</c>.
        /// </value>
        public bool OutsideEEA { get; set; }

        /// <summary>
        /// Gets or sets how automated decision making is used.
        /// </summary>
        public IHtmlString AutomatedDecisionMaking { get; set; }

        /// <summary>
        /// Gets or sets the legal basis for processsing.
        /// </summary>
        public IHtmlString LegalBasis { get; set; }

        /// <summary>
        /// Gets or sets how long personal information is kept.
        /// </summary>
        public IHtmlString HowLong { get; set; }

        /// <summary>
        /// Gets or sets how information might be shared on a 'need to know' basis
        /// </summary>
        public IHtmlString SharingNeedToKnow { get; set; }

        /// <summary>
        /// Gets or sets how information might be shared with third party companies
        /// </summary>
        public IHtmlString SharingThirdParties { get; set; }

        /// <summary>
        /// Gets or sets contact details for the service.
        /// </summary>
        public IHtmlString Contact { get; set; }
    }
}