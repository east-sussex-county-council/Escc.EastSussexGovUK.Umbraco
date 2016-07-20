using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.Latest
{
    /// <summary>
    /// Latest tab for displaying and cascading updates, shared by several groups of document types
    /// </summary>
    public class LatestTab : TabBase
    {
        [UmbracoProperty("Latest", "latest", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: LatestDataType.DataTypeName, sortOrder: 0)]
        public string Latest { get; set; }

        [UmbracoProperty("Latest: publish date", "latestPublishDate", BuiltInUmbracoDataTypes.Date, sortOrder: 1)]
        public string LatestPublishDate { get; set; }

        [UmbracoProperty("Latest: unpublish date", "latestUnpublishDate", BuiltInUmbracoDataTypes.Date, sortOrder: 2)]
        public string LatestUnpublishDate { get; set; }

        [UmbracoProperty("Latest: inherit?", "latestInherit", PropertyEditorAliases.BooleanPropertyEditor, "Checkbox (true by default)", sortOrder: 3)]
        public string LatestInherit { get; set; }

        [UmbracoProperty("Latest: cascade", "latestCascade", PropertyEditorAliases.BooleanPropertyEditor, "Checkbox (true by default)", sortOrder: 4)]
        public string LatestCascade { get; set; }
    }
}