using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan
{
    public class CouncilPlanPrioritiesPageContentTab : TabBase
    {
        [UmbracoProperty("Page Title", "phDefTitle", BuiltInUmbracoDataTypes.Textbox, mandatory: true, sortOrder: 1)]
        public string PhDefTitle { get; set; }

        [UmbracoProperty("Priority", "phDefPriorityClass", BuiltInUmbracoDataTypes.DropDown, PriorityDataType.DataTypeName, sortOrder: 2)]
        public string PhDefPriorityClass { get; set; }

        [UmbracoProperty("Content", "phDefContent", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string PhDefContent { get; set; }

        // Priority 1
        [UmbracoProperty("Priority 1 Title", "phDefPriority1", BuiltInUmbracoDataTypes.Textbox, sortOrder: 4)]
        public string PhDefPriority1 { get; set; }
        [UmbracoProperty("Objectives 1 Content", "phDefObjectives1", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 5)]
        public string PhDefObjectives1 { get; set; }

        // economic-growth
        [UmbracoProperty("Priority 2 Title", "phDefPriority2", BuiltInUmbracoDataTypes.Textbox, sortOrder: 6)]
        public string PhDefPriority2 { get; set; }
        [UmbracoProperty("Objectives 2 Content", "phDefObjectives2", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 7)]
        public string PhDefObjectives2 { get; set; }

        // image
        [UmbracoProperty("SVG file", "phDefSvg", BuiltInUmbracoDataTypes.Textbox, SortOrder = 8, Description = "Choose the SVG file")]
        public string PhDefSvg { get; set; }

        [UmbracoProperty("Fallback image", "phDefFallbackImage", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 9, Description = "")]
        public string PhDefFallbackImage { get; set; }

        [UmbracoProperty("Fallback image map", "phDefFallbackHtml", BuiltInUmbracoDataTypes.TextboxMultiple, SortOrder = 10, Description = "")]
        public string PhDefFallbackHtml { get; set; }

        // best-use-of-resources
        [UmbracoProperty("Priority 3 Title", "phDefPriority3", BuiltInUmbracoDataTypes.Textbox, sortOrder: 11)]
        public string PhDefPriority3 { get; set; }
        [UmbracoProperty("Objectives 3 Content", "phDefObjectives3", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 12)]
        public string PhDefObjectives3 { get; set; }

        // vulnerable-people
        [UmbracoProperty("Priority 4 Title", "phDefPriority4", BuiltInUmbracoDataTypes.Textbox, sortOrder: 13)]
        public string PhDefPriority4 { get; set; }
        [UmbracoProperty("Objectives 4 Content", "phDefObjectives4", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 14)]
        public string PhDefObjectives4 { get; set; }

        [UmbracoProperty("Related pages", "phDefRelatedPages", PropertyEditorAliases.RichTextPropertyEditor, RichTextLinksListDataType.DataTypeName, sortOrder: 15)]
        public string PhDefRelatedPages { get; set; }

        [UmbracoProperty("Related websites", "phDefRelatedSites", PropertyEditorAliases.RichTextPropertyEditor, RichTextLinksListDataType.DataTypeName, sortOrder: 16)]
        public string PhDefRelatedSites { get; set; }
    }
}