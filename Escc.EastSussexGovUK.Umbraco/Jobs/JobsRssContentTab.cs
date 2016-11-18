using System;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Content tab for <see cref="JobsRssDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class JobsRssContentTab : TabBase
    {
        [UmbracoProperty("Search results script URL", "ResultsScriptUrl", UrlDataType.PropertyEditorAlias, UrlDataType.DataTypeName, sortOrder: 1, mandatory: true,
            Description = "A standard TalentLink component is embedded into a page by referencing a script. Paste the URL of the search results component here.")]
        public Uri ScriptUrl { get; set; }

        /// <summary>
        /// Gets or sets the job detail page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the detail page
        /// </value>
        [UmbracoProperty("Job detail page", "JobDetailPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 2, 
            Description = "Select the job detail page used to display a single job advert, based on the 'Jobs component' document type")]
        public string JobDetailPage { get; set; }
    }
}