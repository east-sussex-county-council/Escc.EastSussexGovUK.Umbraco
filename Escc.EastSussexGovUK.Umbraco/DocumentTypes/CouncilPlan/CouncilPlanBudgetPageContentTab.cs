using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan
{
    public class CouncilPlanBudgetPageContentTab : TabBase
    {
        [UmbracoProperty("Page Title", "phDefTitle", BuiltInUmbracoDataTypes.Textbox, mandatory: true, sortOrder: 1)]
        public string PhDefTitle { get; set; }

        [UmbracoProperty("Priority", "phDefPriorityClass", BuiltInUmbracoDataTypes.DropDown, PriorityDataType.DataTypeName, sortOrder: 2)]
        public string PhDefPriorityClass { get; set; }

        [UmbracoProperty("Content", "phDefContent", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string PhDefContent { get; set; }

        [UmbracoProperty("SVG file", "phDefSvg", BuiltInUmbracoDataTypes.MediaPicker, mandatory: true, SortOrder = 4, Description = "Choose the SVG file")]
        public string PhDefSvg { get; set; }

        [UmbracoProperty("Fallback image", "phDefFallbackImage", BuiltInUmbracoDataTypes.MediaPicker, mandatory: true, SortOrder = 5, Description = "")]
        public string PhDefFallbackImage { get; set; }

        [UmbracoProperty("Fallback image map", "phDefFallbackHtml", BuiltInUmbracoDataTypes.TextboxMultiple, mandatory: true, SortOrder = 6, Description = "")]
        public string PhDefFallbackHtml { get; set; }

        [UmbracoProperty("Related pages", "phDefRelatedPages", PropertyEditorAliases.RichTextPropertyEditor, RichTextLinksListDataType.DataTypeName, sortOrder: 7)]
        public string PhDefRelatedPages { get; set; }

        [UmbracoProperty("Related websites", "phDefRelatedSites", PropertyEditorAliases.RichTextPropertyEditor, RichTextLinksListDataType.DataTypeName, sortOrder: 8)]
        public string PhDefRelatedSites { get; set; }
    }
}