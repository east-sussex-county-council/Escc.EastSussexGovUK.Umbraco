using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Content fields for <see cref="JobsHomeDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class JobsHomeContentTab : TabBase
    {
        /// <summary>
        /// Gets or sets the jobs service logo
        /// </summary>
        /// <value>
        /// The jobs logo.
        /// </value>
        [UmbracoProperty("Logo", "JobsLogo", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 1, Description="Select the logo for the jobs service")]
        public string JobsLogo { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header on small screens
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image (small screens)", "HeaderBackgroundImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 2,
            Description = "Select the background image for the page header on small screens such as mobiles. Tiles horizontally up to 474px wide, or until overridden by an image for larger screens.")]
        public string HeaderBackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header on medium screens
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image (medium screens)", "HeaderBackgroundImageMedium", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 3,
            Description = "Select the background image for the page header on medium screens such as tablets. Tiles horizontally up to 802px wide, or until overridden by an image for larger screens.")]
        public string HeaderBackgroundImageMedium { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image (large screens)", "HeaderBackgroundImageLarge", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 4,
            Description = "Select the background image for the page header on large screens such as laptops.")]
        public string HeaderBackgroundImageLarge { get; set; }

        /// <summary>
        /// Gets or sets the optional caption for the background image for the header
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image caption", "HeaderBackgroundImageCaption", BuiltInUmbracoDataTypes.Textbox, sortOrder: 5,
            Description = "Add a caption for the header background image (optional).")]
        public string HeaderBackgroundImageCaption { get; set; }

        /// <summary>
        /// Gets or sets the login page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the login page
        /// </value>
        [UmbracoProperty("Login page", "LoginPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 6, Description = "Select the jobs login page, based on the 'Jobs component' document type")]
        public string LoginPage { get; set; }

        /// <summary>
        /// Gets or sets the search page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the search page
        /// </value>
        [UmbracoProperty("Search page", "SearchPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 7, Description = "Select the search page, based on the 'Job search' document type")]
        public string SearchPage { get; set; }

        /// <summary>
        /// Gets or sets the search results page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the results page
        /// </value>
        [UmbracoProperty("Search results page", "SearchResultsPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 8, Description = "Select the search results page, based on the 'Job search results' document type")]
        public string SearchResultsPage { get; set; }

        /// <summary>
        /// Gets or sets the button navigation.
        /// </summary>
        /// <value>
        /// The button navigation.
        /// </value>
        [UmbracoProperty("Tile navigation", "TileNavigation", BuiltInUmbracoDataTypes.RelatedLinks, sortOrder: 9,
            Description = "Promotional tiles which can be customised using the images below. Set the caption to a hyphen to link the image without text.")]
        public string TileNavigation { get; set; }

        /// <summary>
        /// Gets or sets the images to be linked using <see cref="TileNavigation"/>
        /// </summary>
        [UmbracoProperty("Tile images", "TileImages", BuiltInUmbracoDataTypes.MultipleMediaPicker, sortOrder: 10,
            Description = "Select the images to link to pages selected for tile navigation, above.")]
        public string TileImages { get; set; }

        /// <summary>
        /// Gets or sets the campaign image
        /// </summary>
        [UmbracoProperty("Campaign image", "CampaignImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 11,
            Description = "Select a campaign image to appear below the tile images.")]
        public string CampaignImage { get; set; }

        /// <summary>
        /// Gets or sets the page to link the campaign image to
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the campaign page
        /// </value>
        [UmbracoProperty("Campaign page", "CampaignPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 12,
            Description = "Select a page for the campaign image to link to")]
        public string CampaignPage { get; set; }
    }
}