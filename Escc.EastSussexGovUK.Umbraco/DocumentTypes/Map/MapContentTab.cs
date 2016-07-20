using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map
{
    public class MapContentTab : TabBase
    {
        [UmbracoProperty("Introductory text", "phDefIntro", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string PhDefIntro { get; set; }

        [UmbracoProperty("Map", "phDefMap", BuiltInUmbracoDataTypes.MediaPicker, description: "Select the map here. It must be no wider than 455 pixels.", sortOrder: 4)] 
        public string PhDefMap { get; set; }

        [UmbracoProperty("Image map HTML", "phDefImageMapXhtml", BuiltInUmbracoDataTypes.TextboxMultiple, null, null, false, "Add clickable areas on the map using an HTML 'map' element", 5, true, ValidationRegularExpression = @"^(|\s*<map[^>]*>(\s*<area[^>]*>\s*)+</map>)\s*$")] 
        public string PhDefImageMapXhtml { get; set; }

        [UmbracoProperty("Text description", "phDefContent01", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, description: "Information shown on this map should be given in text as well (required for accessibility). For example, a map showing household waste sites in East Sussex should also list those sites.", mandatory: true, sortOrder: 6)]
        public string PhDefContent01 { get; set; }

        [UmbracoProperty("Related pages", "phDefRelatedPages", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 98)]
        public string PhDefRelatedPages { get; set; }

        [UmbracoProperty("Related websites", "phDefRelatedSites", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 99)]
        public string PhDefRelatedSites { get; set; }
    }
}