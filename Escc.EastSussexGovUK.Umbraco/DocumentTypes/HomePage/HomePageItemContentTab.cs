using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.HomePage
{
    public class HomePageItemContentTab : TabBase
    {
        [UmbracoProperty("URL to link to", "URL", UrlDataType.PropertyEditorAlias, dataTypeInstanceName: UrlDataType.DataTypeName, SortOrder = 1)]
        public string Title { get; set; }

        [UmbracoProperty("Description", "ItemDescription", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccStandardDataType.DataTypeName, description: "Add a short description", SortOrder = 2)]
        public string Description { get; set; }

        [UmbracoProperty("Image", "Image", BuiltInUmbracoDataTypes.MediaPicker, SortOrder = 3, Description= "Your image should be 236px wide by 198px high.")]
        public string Image { get; set; }
    }
}