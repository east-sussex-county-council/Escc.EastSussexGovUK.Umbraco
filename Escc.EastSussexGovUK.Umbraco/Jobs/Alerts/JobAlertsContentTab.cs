using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Content tab for <see cref="JobAlertsDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class JobAlertsContentTab : TabBase
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
        /// Gets or sets the background image for the header on small screens
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image (small screens)", "HeaderBackgroundImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 3,
            Description = "Select the background image for the page header on small screens such as mobiles. Tiles horizontally up to 474px wide, or until overridden by an image for larger screens.")]
        public string HeaderBackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header on medium screens
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image (medium screens)", "HeaderBackgroundImageMedium", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 4,
            Description = "Select the background image for the page header on medium screens such as tablets. Tiles horizontally up to 802px wide, or until overridden by an image for larger screens.")]
        public string HeaderBackgroundImageMedium { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image (large screens)", "HeaderBackgroundImageLarge", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 5,
            Description = "Select the background image for the page header on large screens such as laptops.")]
        public string HeaderBackgroundImageLarge { get; set; }

        /// <summary>
        /// Gets or sets the login page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the login page
        /// </value>
        [UmbracoProperty("Login page", "LoginPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 6, Description = "Select the jobs login page, based on the 'Jobs component' document type")]
        public string LoginPage { get; set; }

        [UmbracoProperty("Which jobs should this page show?", "PublicOrRedeployment", PublicOrRedeploymentDataType.PropertyEditor, PublicOrRedeploymentDataType.DataTypeName, sortOrder: 8, mandatory: true)]
        public string PublicOrRedeploymentJobs { get; set; }
    }
}