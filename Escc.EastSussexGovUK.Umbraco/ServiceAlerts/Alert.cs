using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.ServiceAlerts
{
    /// <summary>
    /// A specification for the 'Alert' document type in Umbraco
    /// </summary>
    [UmbracoContentType("Alert", "alert", null, false, BuiltInUmbracoContentTypeIcons.SprTreeFolder)]
    public class Alert : UmbracoGeneratedBase
    {
        [UmbracoTab("Content")]
        public AlertContentTab Content { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}
