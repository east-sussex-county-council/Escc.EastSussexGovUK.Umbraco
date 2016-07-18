using Escc.EastSussexGovUK.UmbracoDocumentTypes.DataTypes;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person
{
    public class PersonContentTab : TabBase
    {
        [UmbracoProperty("Title", "HonorificTitle", BuiltInUmbracoDataTypes.Textbox, sortOrder: 0, Description="For example, Dr or Cllr")]
        public string HonoraryTitle { get; set; }

        [UmbracoProperty("First name", "GivenName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 1, mandatory: true)]
        public string GivenName { get; set; }

        [UmbracoProperty("Last name", "FamilyName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 2, mandatory: true)]
        public string FamilyName { get; set; }

        [UmbracoProperty("Suffix", "HonorificSuffix", BuiltInUmbracoDataTypes.Textbox, sortOrder: 3, Description = "For example, PhD or MBE")]
        public string HonorarySuffix { get; set; }

        [UmbracoProperty("Job title", "JobTitle", BuiltInUmbracoDataTypes.Textbox, sortOrder: 4, mandatory: true)]
        public string JobTitle { get; set; }

        [UmbracoProperty("Leading text", "leadingText", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 5)]
        public string LeadingText { get; set; }

        [UmbracoProperty("Subheading 1", "subheading1", BuiltInUmbracoDataTypes.Textbox, sortOrder: 6)]
        public string Subheading1 { get; set; }

        [UmbracoProperty("Content 1", "content1", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 7)]
        public string Content1 { get; set; }

        [UmbracoProperty("Subheading 2", "subheading2", BuiltInUmbracoDataTypes.Textbox, sortOrder: 8)]
        public string Subheading2 { get; set; }

        [UmbracoProperty("Content 2", "content2", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 9)]
        public string Content2 { get; set; }

        [UmbracoProperty("Email", "email", EmailAddressDataType.PropertyEditorAlias, EmailAddressDataType.DataTypeName, sortOrder: 10)]
        public string Email { get; set; }

        [UmbracoProperty("Phone", "phone", PhoneNumberDataType.PropertyEditorAlias, PhoneNumberDataType.DataTypeName, sortOrder: 11)]
        public string Phone { get; set; }

        [UmbracoProperty("Additional contact information", "contact", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 12)]
        public string Content3 { get; set; }

        [UmbracoProperty("Photo", "photo", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 13)]
        public string Photo { get; set; }

        [UmbracoProperty("Related links", "relatedLinks", BuiltInUmbracoDataTypes.RelatedLinks, sortOrder: 14, 
            Description="A caption with an empty link will be treated as a heading. If you do not start with a heading, a default heading will be displayed.")]
        public string RelatedLinks { get; set; }
    }
}