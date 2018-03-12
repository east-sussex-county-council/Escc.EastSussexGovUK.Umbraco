using System;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.ServiceAlerts
{
    /// <summary>
    /// Specification for the 'Alerts' document type in Umbraco
    /// </summary>
    [UmbracoContentType("Alerts", "alerts", new Type[] { typeof(Alert) }, true, allowAtRoot: true, enableListView: true, icon: BuiltInUmbracoContentTypeIcons.IconRadio)]
    public class Alerts : UmbracoGeneratedBase
    {
        [UmbracoProperty("Page URL", "umbracoUrlName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 0)]
        public Uri UmbracoUrlName { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}
