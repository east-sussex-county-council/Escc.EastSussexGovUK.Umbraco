using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Skins
{
    /// <summary>
    /// Specification for the 'Skins' document type in Umbraco
    /// </summary>  
    [UmbracoContentType("Skins", "Skins", new Type[] { typeof(SkinDocumentType) }, false, allowAtRoot: true, enableListView: true, 
        icon: BuiltInUmbracoContentTypeIcons.IconPalette, 
        Description = "Manage where skins are applied to pages.")]
    public class SkinsDocumentType : UmbracoGeneratedBase
    {
        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}