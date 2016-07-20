using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan
{
    public class CouncilPlanMonitoringPageContentTab : TabBase
    {
        [UmbracoProperty("Page Title", "phDefTitle", BuiltInUmbracoDataTypes.Textbox, mandatory: true, sortOrder: 1)]
        public string PhDefTitle { get; set; }

        [UmbracoProperty("Priority", "phDefPriorityClass", BuiltInUmbracoDataTypes.DropDown, PriorityDataType.DataTypeName, sortOrder: 2)]
        public string PhDefPriorityClass { get; set; }

        [UmbracoProperty("Content", "phDefContent", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string PhDefContent { get; set; }

        [UmbracoProperty("Related pages", "phDefRelatedPages", PropertyEditorAliases.RichTextPropertyEditor, RichTextLinksListDataType.DataTypeName, sortOrder: 4)]
        public string PhDefRelatedPages { get; set; }

        [UmbracoProperty("Related websites", "phDefRelatedSites", PropertyEditorAliases.RichTextPropertyEditor, RichTextLinksListDataType.DataTypeName, sortOrder: 5)]
        public string PhDefRelatedSites { get; set; }
    }
}