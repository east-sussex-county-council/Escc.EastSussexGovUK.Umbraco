using System;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Definition for the Umbraco 'Problem jobs RSS feed' document type
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase.CustomerFocusBaseDocumentType" />
    [UmbracoContentType("Problem jobs RSS feed", "ProblemJobsRss", new Type[]{}, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconRss, 
    Description = "RSS feed of jobs with missing data, used to alert personnel staff.")]
    public class ProblemJobsRssDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTab("Content")]
        public JobsRssContentTab Content { get; set; }

        [UmbracoProperty("Page URL", "umbracoUrlName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 0)]
        public Uri UmbracoUrlName { get; set; }

        [UmbracoProperty("Description", "pageDescription", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 1, mandatory: true)]
        public string Description { get; set; }

        [UmbracoProperty("Author notes", "phDefAuthorNotes", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }

    }
}