using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
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

        [UmbracoProperty("Latest: publish date", "latestPublishDate", BuiltInUmbracoDataTypes.DateTime, sortOrder: 1)]
        public string LatestPublishDate { get; set; }

        [UmbracoProperty("Latest: unpublish date", "latestUnpublishDate", BuiltInUmbracoDataTypes.DateTime, sortOrder: 2)]
        public string LatestUnpublishDate { get; set; }

        [UmbracoProperty("Latest: inherit?", "latestInherit", CheckboxDataType.PropertyEditorAlias, CheckboxDataType.DataTypeName, sortOrder: 3)]
        public string LatestInherit { get; set; }

        [UmbracoProperty("Latest: cascade", "latestCascade", CheckboxDataType.PropertyEditorAlias, CheckboxDataType.DataTypeName, sortOrder: 4)]
        public string LatestCascade { get; set; }
    }
}