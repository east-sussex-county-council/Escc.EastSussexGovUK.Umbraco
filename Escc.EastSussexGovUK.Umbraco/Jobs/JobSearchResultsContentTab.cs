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
        [UmbracoProperty("Job advert page", "JobDetailPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 6,
            Description = "Select the job detail page used to display a single job advert, based on the 'Job advert' document type")]
        public string JobDetailPage { get; set; }

        /// <summary>
        /// Gets or sets the job search page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the search page
        /// </value>
        [UmbracoProperty("Jobs search page", "JobsSearchPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 7,
            Description = "Select the jobs search page, based on the 'Jobs search' document type")]
        public string JobsSearchPage { get; set; }

        [UmbracoProperty("Which jobs should this page show?", "PublicOrRedeployment", PublicOrRedeploymentDataType.PropertyEditor, PublicOrRedeploymentDataType.DataTypeName, sortOrder: 7, mandatory: true)]
        public string PublicOrRedeploymentJobs { get; set; }
    }
}