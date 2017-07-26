using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Ratings
{
    /// <summary>
    /// Specification for the 'Ratings' document type in Umbraco
    /// </summary>  
    [UmbracoContentType("Ratings", "Ratings", new Type[] { typeof(RatingDocumentType) }, false, allowAtRoot: true, enableListView: true, 
        icon: BuiltInUmbracoContentTypeIcons.IconRate, 
        Description = "Manage page ratings.")]
    public class RatingsDocumentType : UmbracoGeneratedBase
    {
        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}