using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage
{
    public class StandardLandingPageContentTab : TabBase
    {
        private const string DescriptionForDescriptionField = "This will be formatted as a list of links or text description, depending on the 'Descriptions' setting on the 'Properties' tab. Check first to avoid losing your work.";

        [UmbracoProperty("Section navigation", "highlightContent", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextTwoListsOfLinksDataType.DataTypeName, description: "Add links to other pages in this section. Rarely needed.", sortOrder: 1)]
        public string HighlightContent { get; set; }

        [UmbracoProperty("Section navigation image", "highlightImage", BuiltInUmbracoDataTypes.MediaPicker, description: "Image next to links to other pages in this section. Rarely needed.", sortOrder: 2)]
        public string HighlightImage { get; set; }

        [UmbracoProperty("Introductory text", "phDefIntro", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string PhDefIntro { get; set; }

        [UmbracoProperty("Image", "defImage", BuiltInUmbracoDataTypes.MediaPicker, description: "An optional image to go with the introductory text", sortOrder: 4)]
        public string DefImage { get; set; }

        [UmbracoProperty("List title 1", "phDefListTitle01", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 5)]
        public string PhDefListTitle01 { get; set; }

        [UmbracoProperty("Description 1", "defDesc01", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 6, description: DescriptionForDescriptionField)]
        public string DefDesc01 { get; set; }

        [UmbracoProperty("List title 2", "phDefListTitle02", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 7)]
        public string PhDefListTitle02 { get; set; }

        [UmbracoProperty("Description 2", "defDesc02", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 8, description: DescriptionForDescriptionField)]
        public string DefDesc02 { get; set; }

        [UmbracoProperty("List title 3", "phDefListTitle03", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 10)]
        public string PhDefListTitle03 { get; set; }

        [UmbracoProperty("Description 3", "defDesc03", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 11, description: DescriptionForDescriptionField)]
        public string DefDesc03 { get; set; }

        [UmbracoProperty("List title 4", "phDefListTitle04", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 13)]
        public string PhDefListTitle04 { get; set; }

        [UmbracoProperty("Description 4", "defDesc04", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 14, description: DescriptionForDescriptionField)]
        public string DefDesc04 { get; set; }

        [UmbracoProperty("List title 5", "phDefListTitle05", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 16)]
        public string PhDefListTitle05 { get; set; }

        [UmbracoProperty("Description 5", "defDesc05", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 17, description: DescriptionForDescriptionField)]
        public string DefDesc05 { get; set; }

        [UmbracoProperty("List title 6", "phDefListTitle06", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 19)]
        public string PhDefListTitle06 { get; set; }

        [UmbracoProperty("Description 6", "defDesc06", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 20, description: DescriptionForDescriptionField)]
        public string DefDesc06 { get; set; }

        [UmbracoProperty("List title 7", "phDefListTitle07", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 22)]
        public string PhDefListTitle07 { get; set; }

        [UmbracoProperty("Description 7", "defDesc07", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 23, description: DescriptionForDescriptionField)]
        public string DefDesc07 { get; set; }

        [UmbracoProperty("List title 8", "phDefListTitle08", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 25)]
        public string PhDefListTitle08 { get; set; }

        [UmbracoProperty("Description 8", "defDesc08", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 26, description: DescriptionForDescriptionField)]
        public string DefDesc08 { get; set; }

        [UmbracoProperty("List title 9", "phDefListTitle09", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 28)]
        public string PhDefListTitle09 { get; set; }

        [UmbracoProperty("Description 9", "defDesc09", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 29, description: DescriptionForDescriptionField)]
        public string DefDesc09 { get; set; }

        [UmbracoProperty("List title 10", "phDefListTitle10", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 31)]
        public string PhDefListTitle10 { get; set; }

        [UmbracoProperty("Description 10", "defDesc10", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 32, description: DescriptionForDescriptionField)]
        public string DefDesc10 { get; set; }

        [UmbracoProperty("List title 11", "phDefListTitle11", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 34)]
        public string PhDefListTitle11 { get; set; }

        [UmbracoProperty("Description 11", "defDesc11", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 35, description: DescriptionForDescriptionField)]
        public string DefDesc11 { get; set; }

        [UmbracoProperty("List title 12", "phDefListTitle12", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 37)]
        public string PhDefListTitle12 { get; set; }

        [UmbracoProperty("Description 12", "defDesc12", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 38, description: DescriptionForDescriptionField)]
        public string DefDesc12 { get; set; }

        [UmbracoProperty("List title 13", "phDefListTitle13", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 40)]
        public string PhDefListTitle13 { get; set; }

        [UmbracoProperty("Description 13", "defDesc13", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 41, description: DescriptionForDescriptionField)]
        public string DefDesc13 { get; set; }

        [UmbracoProperty("List title 14", "phDefListTitle14", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 43)]
        public string PhDefListTitle14 { get; set; }

        [UmbracoProperty("Description 14", "defDesc14", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 44, description: DescriptionForDescriptionField)]
        public string DefDesc14 { get; set; }

        [UmbracoProperty("List title 15", "phDefListTitle15", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 46)]
        public string PhDefListTitle15 { get; set; }

        [UmbracoProperty("Description 15", "defDesc15", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 47, description: DescriptionForDescriptionField)]
        public string DefDesc15 { get; set; }

        [UmbracoProperty("Related pages", "phDefRelatedPages", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 98)]
        public string PhDefRelatedPages { get; set; }

        [UmbracoProperty("Related websites", "phDefRelatedSites", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 99)]
        public string PhDefRelatedSites { get; set; }
    }
}