using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload
{
    /// <summary>
    /// Properties for populating a form download page
    /// </summary>
    public class FormDownloadContentTab : TabBase
    {
        [UmbracoProperty("Introductory text", "phDefIntro", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string PhDefIntro { get; set; }

        [UmbracoProperty("Guidance notes", "phDefGuidance", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, SortOrder = 4)]
        public string PhDefGuidance { get; set; }

        [UmbracoProperty("Guidance attachment 1", "phDefGuidance1", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 5, Description = "Choose a guidance document in .pdf format")]
        public string PhDefGuidance1 { get; set; }

        [UmbracoProperty("Guidance attachment 2", "phDefGuidance2", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 6, Description = "Choose a guidance document in .pdf format")]
        public string PhDefGuidance2 { get; set; }

        [UmbracoProperty("Guidance attachment 3", "phDefGuidance3", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 7, Description = "Choose a guidance document in .pdf format")]
        public string PhDefGuidance3 { get; set; }

        [UmbracoProperty("Guidance attachment 4", "phDefGuidance4", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 8, Description = "Choose a guidance document in .pdf format")]
        public string PhDefGuidance4 { get; set; }

        [UmbracoProperty("Guidance attachment 5", "phDefGuidance5", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 9, Description = "Choose a guidance document in .pdf format")]
        public string PhDefGuidance5 { get; set; }

        [UmbracoProperty("Address of online form", "phDefXhtmlUrl", PropertyEditorAliases.UrlPropertyEditor, dataTypeInstanceName: UrlDataType.DataTypeName, description: "eg https://www.eastsussex.gov.uk/my-form", SortOrder = 10)]
        public Uri PhDefXhtmlUrl { get; set; }

        [UmbracoProperty("Address of upload page for downloaded forms", "phDefUploadUrl", PropertyEditorAliases.UrlPropertyEditor, dataTypeInstanceName: UrlDataType.DataTypeName, description: "eg https://www.eastsussex.gov.uk/upload-my-form", SortOrder = 11)] 
        public Uri PhDefUploadUrl { get; set; }  
     
        [UmbracoProperty("Print, write and post a PDF", "phDefPdf", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 12, Description = "Choose an Adobe Acrobat file in .pdf format")]
        public string PhDefPdf { get; set; }

        [UmbracoProperty("Print, write and post an Excel file", "phDefXlsPrint", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 13, Description = "Choose an Excel file in .xls format")]
        public string PhDefXlsPrint { get; set; }

        [UmbracoProperty("Type, sign and post an RTF", "phDefRtfSign", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 14, Description = "Choose a Word form saved in .rtf format")]
        public string PhDefRtfSign { get; set; }

        [UmbracoProperty("Email an RTF to us", "phDefRtf", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 15, Description = "Choose a Word form saved in .rtf format")]
        public string PhDefRtf { get; set; }
        
        [UmbracoProperty("Email an Excel file to us", "phDefXls", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 16, Description = "Choose an Excel file in .xls format")]
        public string PhDefXls { get; set; }

        [UmbracoProperty("Footnote", "phDefFootnote", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 85)]
        public string PhDefFootnote { get; set; }

        [UmbracoProperty("Related pages", "phDefRelatedPages", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 98)]
        public string PhDefRelatedPages { get; set; }

        [UmbracoProperty("Related websites", "phDefRelatedSites", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 99)]
        public string PhDefRelatedSites { get; set; }
    }
}