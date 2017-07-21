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
    /// Specification for the 'Rating' document type in Umbraco
    /// </summary>  
    [UmbracoContentType("Rating", "Rating", new Type[0], false, allowAtRoot: false, enableListView: false, icon: BuiltInUmbracoContentTypeIcons.IconRate, 
        Description = "A rating tracker which can be applied to one or more pages.")]
    public class RatingDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTab("Content")]
        public RatingContentTab Content { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}