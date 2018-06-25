using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.Latest;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.SocialMedia;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase
{
    [UmbracoContentType("Legacy Base", "LegacyBase", null, false)]
    public class LegacyBaseDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTab("Latest", SortOrder = 1)]
        public LatestTab LatestTab { get; set; }

        [UmbracoTab("Social media and promotion", SortOrder = 2)]
        public SocialMediaAndPromotionTab SocialMedia { get; set; }

        [UmbracoProperty("Page URL", "umbracoUrlName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 0)]
        public Uri UmbracoUrlName { get; set; }

        [UmbracoProperty("Description", "pageDescription", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 1, mandatory: true)]
        public string Description { get; set; }

        [UmbracoProperty("Author notes", "phDefAuthorNotes", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }

        [UmbracoProperty("IPSV preferred", "ipsvPreferred", BuiltInUmbracoDataTypes.Textbox, sortOrder: 3)]
        public string IpsvPreferred { get; set; }

        [UmbracoProperty("Cache", "cache", BuiltInUmbracoDataTypes.DropDown, "Cache", sortOrder: 101, description: "Pages are cached for 24 hours by default.  If this page is particularly time-sensitive, pick a shorter time.")]
        public string Cache { get; set; }
    }
}