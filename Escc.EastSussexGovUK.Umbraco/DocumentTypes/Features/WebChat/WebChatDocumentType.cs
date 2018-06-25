using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.WebChat
{
    /// <summary>
    /// A specification for the 'Web chat' document type in Umbraco
    /// </summary>
    [UmbracoContentType("Web chat", "WebChat", null, false, BuiltInUmbracoContentTypeIcons.IconChat, allowAtRoot:true)]
    public class WebChatDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTab("Content")]
        public WebChatContentTab Content { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}
