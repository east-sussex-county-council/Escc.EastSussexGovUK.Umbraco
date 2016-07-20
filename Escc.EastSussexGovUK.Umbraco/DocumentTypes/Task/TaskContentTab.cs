using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task
{
    public class TaskContentTab : TabBase
    {
        [UmbracoProperty("Leading text", "leadingText", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 0)]
        public string LeadingText { get; set; }

        [UmbracoProperty("Button: target URL", "startPageUrl", UrlDataType.PropertyEditorAlias, UrlDataType.DataTypeName, sortOrder: 1, mandatory: true)]
        public Uri StartPageUrl { get; set; }

        [UmbracoProperty("Button: text", "startButtonText", BuiltInUmbracoDataTypes.Textbox, sortOrder: 1, mandatory: true)]
        public string StartButtonText { get; set; }

        [UmbracoProperty("Subheading 1", "subheading1", BuiltInUmbracoDataTypes.Textbox, sortOrder: 2)]
        public string Subheading1 { get; set; }

        [UmbracoProperty("Content 1", "content1", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string Content1 { get; set; }

        [UmbracoProperty("Subheading 2", "subheading2", BuiltInUmbracoDataTypes.Textbox, sortOrder: 4)]
        public string Subheading2 { get; set; }

        [UmbracoProperty("Content 2", "content2", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 5)]
        public string Content2 { get; set; }

        [UmbracoProperty("Subheading 3", "subheading3", BuiltInUmbracoDataTypes.Textbox, sortOrder: 6)]
        public string Subheading3 { get; set; }

        [UmbracoProperty("Content 3", "content3", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 7)]
        public string Content3 { get; set; }

        [UmbracoProperty("Subheading 4", "subheading4", BuiltInUmbracoDataTypes.Textbox, sortOrder: 8)]
        public string Subheading4 { get; set; }

        [UmbracoProperty("Content 4", "content4", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 9)]
        public string Content4 { get; set; }

        [UmbracoProperty("Partner images", "partnerImages", BuiltInUmbracoDataTypes.MultipleMediaPicker, sortOrder: 10)]
        public string PartnerImages { get; set; }

        [UmbracoProperty("Related links", "relatedLinks", BuiltInUmbracoDataTypes.RelatedLinks, sortOrder: 11, 
            Description="A caption with an empty link will be treated as a heading. If you do not start with a heading, a default heading will be displayed.")]
        public string RelatedLinks { get; set; }
    }
}