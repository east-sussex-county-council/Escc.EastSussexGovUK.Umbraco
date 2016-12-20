using System;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Content tab for <see cref="JobsComponentDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class JobsSearchContentTab : TabBase
    {
        /// <summary>
        /// Gets or sets the jobs service logo
        /// </summary>
        /// <value>
        /// The jobs logo.
        /// </value>
        [UmbracoProperty("Logo", "JobsLogo", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 1, Description = "Select the logo for the jobs service")]
        public string JobsLogo { get; set; }

        /// <summary>
        /// Gets or sets the jobs start page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the home page
        /// </value>
        [UmbracoProperty("Jobs home page", "JobsHomePage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 2, Description = "Select the jobs home page, to be linked from the logo")]
        public string JobsHomePage { get; set; }

        /// <summary>
        /// Gets or sets the search results page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the results page
        /// </value>
        [UmbracoProperty("Search results page", "SearchResultsPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 3, Description = "Select the search results page, based on the 'Job search results' document type")]
        public string SearchResultsPage { get; set; }

        [UmbracoProperty("Search script URL", "SearchScriptUrl", UrlDataType.PropertyEditorAlias, UrlDataType.DataTypeName, sortOrder: 4, mandatory: true,
        Description = "A standard TalentLink component is embedded into a page by referencing a script. Paste the URL of the search component here.")]
        public Uri SearchScriptUrl { get; set; }
    }
}