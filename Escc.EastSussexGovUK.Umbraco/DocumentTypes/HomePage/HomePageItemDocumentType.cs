using System;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.HomePage
{
    /// <summary>
    /// Specification for the 'Home page item' document type in Umbraco, which is a small piece of content which appears on the home page
    /// </summary>
    [UmbracoContentType("Home page item", "HomePageItem", new Type[0], false, icon: BuiltInUmbracoContentTypeIcons.IconPlugin, Description="A short piece of content used to build up the home page.")]
    public class HomePageItemDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTab("Content", SortOrder=1)]
        public HomePageItemContentTab Content { get; set; }

        [UmbracoProperty("Author notes", "phDefAuthorNotes", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }
    }
}