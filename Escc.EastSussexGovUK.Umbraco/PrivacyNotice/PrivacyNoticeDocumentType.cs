using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.Latest;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
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

        [UmbracoTab("Rights")]
        public PrivacyNoticeRightsTab Rights { get; set; }

        [UmbracoTab("Contact")]
        public PrivacyNoticeContactTab Contact { get; set; }

        [UmbracoTab("Latest")]
        public LatestTab Latest { get; set; }

        [UmbracoProperty("Page URL", "umbracoUrlName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 0)]
        public Uri UmbracoUrlName { get; set; }

        [UmbracoProperty("Description", "pageDescription", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 1, mandatory: true)]
        public string Description { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }

        [UmbracoProperty("Cache", "cache", BuiltInUmbracoDataTypes.DropDown, "Cache", sortOrder: 101, description: "Pages are cached for 24 hours by default. If this page is particularly time-sensitive, pick a shorter time.")]
        public string Cache { get; set; }

        [UmbracoProperty("Copy of unpublished date (do not edit)", "unpublishAt", BuiltInUmbracoDataTypes.DateTime, sortOrder: 103)]
        public string UnpublishAt { get; set; }
    }
}