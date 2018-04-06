using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.Latest;
using System;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    [UmbracoContentType("Privacy notice", "PrivacyNotice", new Type[0], true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconKeychain,
        Description = "A privacy notice, required by UK data protection law for any service which collects personally identifiable information.")]
    public class PrivacyNoticeDocumentType
    {
        [UmbracoTab("What?")]
        public PrivacyNoticeWhatTab What { get; set; }

        [UmbracoTab("Why?")]
        public PrivacyNoticeWhyTab Why { get; set; }

        [UmbracoTab("Legal basis")]
        public PrivacyNoticeLegalBasisTab LegalBasis { get; set; }

        [UmbracoTab("How long?")]
        public PrivacyNoticeHowLongTab HowLong { get; set; }

        [UmbracoTab("Sharing")]
        public PrivacyNoticeSharingTab Sharing { get; set; }

        [UmbracoTab("Contact")]
        public PrivacyNoticeContactTab Contact { get; set; }

        [UmbracoTab("Latest")]
        public LatestTab Latest { get; set; }
    }
}