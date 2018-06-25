using System;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Definition for the Umbraco 'Jobs feed' document type
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase.CustomerFocusBaseDocumentType" />
    [UmbracoContentType("Jobs feed", "JobsRss", new Type[]{}, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconRss, 
    Description = "Data feed of all jobs.")]
    public class JobsRssDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTemplate(DisplayName = "Jobs feed as an HTML table", Alias = "JobsRssAsTable")]
        public string RssAsTable { get; set; }

        [UmbracoTemplate(DisplayName = "Jobs feed as Tweets RSS", Alias = "JobsRssAsTweets")]
        public string FeedAsRssTweets { get; set; }

        [UmbracoTemplate(DisplayName = "Jobs feed as Indeed XML", Alias = "JobsRssAsIndeedXml")]
        public string FeedAsIndeedXml { get; set; }

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