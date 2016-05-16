using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures
{
    public class LandingPageWithPicturesContentTab : TabBase
    {
        [UmbracoProperty("Section navigation", "highlightContent", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextTwoListsOfLinksDataType.DataTypeName, description: "Add links to other pages in this section. Rarely needed.", sortOrder: 1)]
        public string HighlightContent { get; set; }

        [UmbracoProperty("Section navigation image", "highlightImage", BuiltInUmbracoDataTypes.MediaPicker, description: "Image next to links to other pages in this section. Rarely needed.", sortOrder: 2)]
        public string HighlightImage { get; set; }

        [UmbracoProperty("Introductory text", "phDefIntro", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string PhDefIntro { get; set; }

        [UmbracoProperty("List title 1", "phDefListTitle01", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 4)]
        public string PhDefListTitle01 { get; set; }

        [UmbracoProperty("Description 1", "phDefListDesc01", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 5)]
        public string PhDefListDesc01 { get; set; }

        [UmbracoProperty("Image 1", "phDefImage01", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 6)]
        public string PhDefImage01 { get; set; }

        [UmbracoProperty("List title 2", "phDefListTitle02", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 7)]
        public string PhDefListTitle02 { get; set; }

        [UmbracoProperty("Description 2", "phDefListDesc02", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 8)]
        public string PhDefListDesc02 { get; set; }

        [UmbracoProperty("Image 2", "phDefImage02", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 9)]
        public string PhDefImage02 { get; set; }

        [UmbracoProperty("List title 3", "phDefListTitle03", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 10)]
        public string PhDefListTitle03 { get; set; }

        [UmbracoProperty("Description 3", "phDefListDesc03", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 11)]
        public string PhDefListDesc03 { get; set; }

        [UmbracoProperty("Image 3", "phDefImage03", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 12)]
        public string PhDefImage03 { get; set; }

        [UmbracoProperty("List title 4", "phDefListTitle04", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 13)]
        public string PhDefListTitle04 { get; set; }

        [UmbracoProperty("Description 4", "phDefListDesc04", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 14)]
        public string PhDefListDesc04 { get; set; }

        [UmbracoProperty("Image 4", "phDefImage04", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 15)]
        public string PhDefImage04 { get; set; }

        [UmbracoProperty("List title 5", "phDefListTitle05", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 16)]
        public string PhDefListTitle05 { get; set; }

        [UmbracoProperty("Description 5", "phDefListDesc05", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 17)]
        public string PhDefListDesc05 { get; set; }

        [UmbracoProperty("Image 5", "phDefImage05", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 18)]
        public string PhDefImage05 { get; set; }

        [UmbracoProperty("List title 6", "phDefListTitle06", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 19)]
        public string PhDefListTitle06 { get; set; }

        [UmbracoProperty("Description 6", "phDefListDesc06", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 20)]
        public string PhDefListDesc06 { get; set; }

        [UmbracoProperty("Image 6", "phDefImage06", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 21)]
        public string PhDefImage06 { get; set; }

        [UmbracoProperty("List title 7", "phDefListTitle07", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 22)]
        public string PhDefListTitle07 { get; set; }

        [UmbracoProperty("Description 7", "phDefListDesc07", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 23)]
        public string PhDefListDesc07 { get; set; }

        [UmbracoProperty("Image 7", "phDefImage07", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 24)]
        public string PhDefImage07 { get; set; }

        [UmbracoProperty("List title 8", "phDefListTitle08", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 25)]
        public string PhDefListTitle08 { get; set; }

        [UmbracoProperty("Description 8", "phDefListDesc08", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 26)]
        public string PhDefListDesc08 { get; set; }

        [UmbracoProperty("Image 8", "phDefImage08", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 27)]
        public string PhDefImage08 { get; set; }

        [UmbracoProperty("List title 9", "phDefListTitle09", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 28)]
        public string PhDefListTitle09 { get; set; }

        [UmbracoProperty("Description 9", "phDefListDesc09", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 29)]
        public string PhDefListDesc09 { get; set; }

        [UmbracoProperty("Image 9", "phDefImage09", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 30)]
        public string PhDefImage09 { get; set; }

        [UmbracoProperty("List title 10", "phDefListTitle10", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 31)]
        public string PhDefListTitle10 { get; set; }

        [UmbracoProperty("Description 10", "phDefListDesc10", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 32)]
        public string PhDefListDesc10 { get; set; }

        [UmbracoProperty("Image 10", "phDefImage10", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 33)]
        public string PhDefImage10 { get; set; }

        [UmbracoProperty("List title 11", "phDefListTitle11", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 34)]
        public string PhDefListTitle11 { get; set; }

        [UmbracoProperty("Description 11", "phDefListDesc11", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 35)]
        public string PhDefListDesc11 { get; set; }

        [UmbracoProperty("Image 11", "phDefImage11", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 36)]
        public string PhDefImage11 { get; set; }

        [UmbracoProperty("List title 12", "phDefListTitle12", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 37)]
        public string PhDefListTitle12 { get; set; }

        [UmbracoProperty("Description 12", "phDefListDesc12", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 38)]
        public string PhDefListDesc12 { get; set; }

        [UmbracoProperty("Image 12", "phDefImage12", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 39)]
        public string PhDefImage12 { get; set; }

        [UmbracoProperty("List title 13", "phDefListTitle13", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 40)]
        public string PhDefListTitle13 { get; set; }

        [UmbracoProperty("Description 13", "phDefListDesc13", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 41)]
        public string PhDefListDesc13 { get; set; }

        [UmbracoProperty("Image 13", "phDefImage13", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 42)]
        public string PhDefImage13 { get; set; }

        [UmbracoProperty("List title 14", "phDefListTitle14", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 43)]
        public string PhDefListTitle14 { get; set; }

        [UmbracoProperty("Description 14", "phDefListDesc14", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 44)]
        public string PhDefListDesc14 { get; set; }

        [UmbracoProperty("Image 14", "phDefImage14", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 45)]
        public string PhDefImage14 { get; set; }

        [UmbracoProperty("List title 15", "phDefListTitle15", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextSingleLinkDataType.DataTypeName, sortOrder: 46)]
        public string PhDefListTitle15 { get; set; }

        [UmbracoProperty("Description 15", "phDefListDesc15", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, sortOrder: 47)]
        public string PhDefListDesc15 { get; set; }

        [UmbracoProperty("Image 15", "phDefImage15", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 48)]
        public string PhDefImage15 { get; set; }

        [UmbracoProperty("Related pages", "phDefRelatedPages", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 98)]
        public string PhDefRelatedPages { get; set; }

        [UmbracoProperty("Related websites", "phDefRelatedSites", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 99)]
        public string PhDefRelatedSites { get; set; }
    }
}