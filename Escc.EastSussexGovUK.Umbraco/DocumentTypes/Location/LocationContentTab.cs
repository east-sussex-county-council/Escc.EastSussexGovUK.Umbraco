using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location
{
    public class LocationContentTab : TabBase
    {
        [UmbracoProperty("Opening hours", "openingHours", "Jumoo.OpeningSoon", "Opening hours", sortOrder: 1)]
        public DateTime OpeningHours { get; set; }

        [UmbracoProperty("Opening hours details", "openingHoursDetails", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, description: "For example, seasonal variations and holiday closures", sortOrder: 2)]
        public string OpeningHoursDetails { get; set; }

        [UmbracoProperty("Content", "content", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string Content { get; set; }

        [UmbracoProperty("Tab 1 title", "tab1title", BuiltInUmbracoDataTypes.Textbox, sortOrder: 4)]
        public string Tab1Title { get; set; }

        [UmbracoProperty("Tab 1 content", "tab1content", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 5)]
        public string Tab1Content { get; set; }

        [UmbracoProperty("Tab 2 title", "tab2title", BuiltInUmbracoDataTypes.Textbox, sortOrder: 6)]
        public string Tab2Title { get; set; }

        [UmbracoProperty("Tab 2 content", "tab2content", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 7)]
        public string Tab2Content { get; set; }

        [UmbracoProperty("Tab 3 title", "tab3title", BuiltInUmbracoDataTypes.Textbox, sortOrder: 8)]
        public string Tab3Title { get; set; }

        [UmbracoProperty("Tab 3 content", "tab3content", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 9)]
        public string Tab3Content { get; set; }

        [UmbracoProperty("Photo", "photo", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 10)]
        public string Photo { get; set; }

        [UmbracoProperty("Location", "location", PropertyEditorAliases.UkLocationPropertyEditor, SortOrder = 11)]
        public string Location { get; set; }

        [UmbracoProperty("Email 1 label", "email1label", BuiltInUmbracoDataTypes.Textbox, sortOrder: 12)]
        public string Email1Label { get; set; }

        [UmbracoProperty("Email 1", "email1", EmailAddressDataType.PropertyEditorAlias, EmailAddressDataType.DataTypeName, sortOrder: 13)]
        public string Email1 { get; set; }

        [UmbracoProperty("Email 2 label", "email2label", BuiltInUmbracoDataTypes.Textbox, sortOrder: 14)]
        public string Email2Label { get; set; }

        [UmbracoProperty("Email 2", "email2", EmailAddressDataType.PropertyEditorAlias, EmailAddressDataType.DataTypeName, sortOrder: 15)]
        public string Email2 { get; set; }

        [UmbracoProperty("Phone 1 label", "phone1label", BuiltInUmbracoDataTypes.Textbox, sortOrder: 16)]
        public string Phone1Label { get; set; }

        [UmbracoProperty("Phone 1", "phone1", PhoneNumberDataType.PropertyEditorAlias, PhoneNumberDataType.DataTypeName, sortOrder: 17)]
        public string Phone1 { get; set; }

        [UmbracoProperty("Phone 2 label", "phone2label", BuiltInUmbracoDataTypes.Textbox, sortOrder: 18)]
        public string Phone2Label { get; set; }

        [UmbracoProperty("Phone 2", "phone2", PhoneNumberDataType.PropertyEditorAlias, PhoneNumberDataType.DataTypeName, sortOrder: 19)]
        public string Phone2 { get; set; }

        [UmbracoProperty("Fax 1 label", "fax1label", BuiltInUmbracoDataTypes.Textbox, sortOrder: 20)]
        public string Fax1Label { get; set; }

        [UmbracoProperty("Fax 1", "fax1", PhoneNumberDataType.PropertyEditorAlias, PhoneNumberDataType.DataTypeName, sortOrder: 21)]
        public string Fax1 { get; set; }

        [UmbracoProperty("Fax 2 label", "fax2label", BuiltInUmbracoDataTypes.Textbox, sortOrder: 22)]
        public string Fax2Label { get; set; }

        [UmbracoProperty("Fax 2", "fax2", PhoneNumberDataType.PropertyEditorAlias, PhoneNumberDataType.DataTypeName, sortOrder: 23)]
        public string Fax2 { get; set; }

        [UmbracoProperty("Related links", "relatedLinks", BuiltInUmbracoDataTypes.RelatedLinks, sortOrder: 24,
            Description="A caption with an empty link will be treated as a heading. If you do not start with a heading, a default heading will be displayed.")]
        public string RelatedLinks { get; set; }
    }
}