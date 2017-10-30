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
    /// Specification for the 'Content experiment page' document type in Umbraco
    /// </summary>  
    [UmbracoContentType("Skin", "Skin", new Type[0], false, allowAtRoot: false, enableListView: false, icon: BuiltInUmbracoContentTypeIcons.IconBrush, Description = "Apply a custom skin to a section of the content.")]
    public class SkinDocumentType: UmbracoGeneratedBase
    {
        [UmbracoTab("Apply skin")]
        public ApplySkinTab Content { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}