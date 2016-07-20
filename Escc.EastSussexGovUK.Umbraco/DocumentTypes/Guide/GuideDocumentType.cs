using System;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Guide
{
    /// <summary>
    /// Umbraco document type for a guide, comprised of several guide steps
    /// </summary>
    [UmbracoContentType("Guide", "Guide", new Type[]
    {
        typeof(GuideStepDocumentType)
    }, true, allowAtRoot: false, enableListView: true, icon: BuiltInUmbracoContentTypeIcons.IconParentalControl,
       Description = "A series of pages in a specific order, which tells people all they need to know about a subject.")]
    public class GuideDocumentType : UmbracoGeneratedBase
    {
        [UmbracoProperty("Page URL", "umbracoUrlName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 0)]
        public Uri UmbracoUrlName { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }

        [UmbracoProperty("Cache", "cache", BuiltInUmbracoDataTypes.DropDown, "Cache", sortOrder: 101, description: "Pages are cached for 24 hours by default.  If this page is particularly time-sensitive, pick shorter time.")]
        public string Cache { get; set; }

        [UmbracoProperty("Copy of unpublished date (do not edit)", "unpublishAt", BuiltInUmbracoDataTypes.DateTime, sortOrder: 103)]
        public string UnpublishAt { get; set; }
    }
}