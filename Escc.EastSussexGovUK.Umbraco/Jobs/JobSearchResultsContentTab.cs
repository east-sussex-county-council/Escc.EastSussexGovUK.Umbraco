using System;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Content tab for <see cref="JobSearchResultsDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class JobSearchResultsContentTab : TabBase
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
        /// Gets or sets the background image for the header
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image", "HeaderBackgroundImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 2, Description = "Select the background image for the page header")]
        public string HeaderBackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the jobs start page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the home page
        /// </value>
        [UmbracoProperty("Jobs home page", "JobsHomePage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 3, Description = "Select the jobs home page, to be linked from the logo")]
        public string JobsHomePage { get; set; }

        /// <summary>
        /// Gets or sets the login page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the login page
        /// </value>
        [UmbracoProperty("Login page", "LoginPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 4, Description = "Select the jobs login page, based on the 'Jobs component' document type")]
        public string LoginPage { get; set; }

        /// <summary>
        /// Gets or sets the job alerts page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the job alerts page
        /// </value>
        [UmbracoProperty("Job alerts page", "JobAlertsPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 5, Description = "Select the job alerts page, based on the 'Jobs component' document type")]
        public string JobAlertsPage { get; set; }

        /// <summary>
        /// Gets or sets the job detail page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the detail page
        /// </value>
        [UmbracoProperty("Job detail page", "JobDetailPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 6,
            Description = "Select the job detail page used to display a single job advert, based on the 'Jobs component' document type")]
        public string JobDetailPage { get; set; }

        [UmbracoProperty("Search script URL", "SearchScriptUrl", UrlDataType.PropertyEditorAlias, UrlDataType.DataTypeName, sortOrder: 7, mandatory: true,
        Description = "A standard TalentLink component is embedded into a page by referencing a script. Paste the URL of the search component here.")]
        public Uri SearchScriptUrl { get; set; }

        [UmbracoProperty("Search results script URL", "ResultsScriptUrl", UrlDataType.PropertyEditorAlias, UrlDataType.DataTypeName, sortOrder: 8, mandatory: true,
            Description = "A standard TalentLink component is embedded into a page by referencing a script. Paste the URL of the search results component here.")]
        public Uri ResultsScriptUrl { get; set; }

        /// <summary>
        /// Gets or sets the button navigation.
        /// </summary>
        /// <value>
        /// The button navigation.
        /// </value>
        [UmbracoProperty("Button navigation", "ButtonNavigation", BuiltInUmbracoDataTypes.RelatedLinks, sortOrder: 9,
            Description = "Buttons and adverts, which can be customised using the images below. Set the caption to a space or hyphen to link the image without text.")]
        public string ButtonNavigation { get; set; }

        /// <summary>
        /// Gets or sets the images to be linked using <see cref="ButtonNavigation"/>
        /// </summary>
        [UmbracoProperty("Button images", "ButtonImages", BuiltInUmbracoDataTypes.MultipleMediaPicker, sortOrder: 10,
            Description = "Select the images to link to pages selected for button navigation, above.")]
        public string ButtonImages { get; set; }
    }
}