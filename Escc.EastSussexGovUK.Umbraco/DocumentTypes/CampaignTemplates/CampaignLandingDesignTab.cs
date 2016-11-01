using System.ComponentModel;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates
{
    /// <summary>
    /// Design tab for the campaign landing document type
    /// </summary>
    public class CampaignLandingDesignTab : TabBase
    {
        [UmbracoProperty("Background colour", "BackgroundColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 1,
            Description = "This will show below or to the sides of the background image when wrapping is turned off.")]
        public string BackgroundColour { get; set; }

        [UmbracoProperty("Background image (small screens)", "BackgroundSmall", BuiltInUmbracoDataTypes.MediaPicker, sortOrder:2,
            Description="Background image for small screens such as mobiles. Typically seen between 320px and 474px wide, but this will be used on medium and large screens too unless another image is chosen below.")]
        public string BackgroundImageSmall { get; set; }

        [UmbracoProperty("Background image (medium screens)", "BackgroundMedium", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 3,
            Description = "Background image for medium screens such as tablets, if different from the small screen background. Typically seen between 474px and 802px wide, but this will be used on large screens too unless another image is chosen below.")]
        public string BackgroundImageMedium { get; set; }

        [UmbracoProperty("Background image (large screens)", "BackgroundLarge", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 4,
            Description = "Background image for large screens such as laptops, at any size above 802px wide.")]
        public string BackgroundImageLarge { get; set; }

        [UmbracoProperty("Wrap background image horizontally", "BackgroundImageWrapsHorizontally", BuiltInUmbracoDataTypes.Boolean, sortOrder: 5,
            Description = "If the screen is wider than the background image, wrap the image horizontally. If you turn this off, the background colour shows instead.")]
        public string BackgroundImageWrapsHorizontally { get; set; }

        [UmbracoProperty("Wrap background image vertically", "BackgroundImageWrapsVertically", BuiltInUmbracoDataTypes.Boolean, sortOrder: 6,
            Description = "If the page is longer than the background image, wrap the image vertically. If you turn this off, the background colour shows instead.")]
        public string BackgroundImageWrapsVertically { get; set; }

        [UmbracoProperty("Heading text colour", "HeadingColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 7)]
        public string HeadingColour { get; set; }

        [UmbracoProperty("Introduction text colour", "IntroductionColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 8)]
        public string IntroductionColour { get; set; }
        
        /// <summary>
        /// Gets or sets how to align the introduction on medium screens
        /// </summary>
        [UmbracoProperty("Align introduction (medium screens)", "AlignIntroductionMedium", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 9)]
        public string AlignIntroductionMedium { get; set; }

        /// <summary>
        /// Gets or sets how to align the landing navigation on medium screens
        /// </summary>
        [UmbracoProperty("Align landing navigation (medium screens)", "AlignLandingNavigationMedium", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 10)]
        public string AlignLandingNavigationMedium { get; set; }

        /// <summary>
        /// Gets or sets how to align the buttons on medium screens
        /// </summary>
        [UmbracoProperty("Align buttons (medium screens)", "AlignButtonsMedium", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 11)]
        public string AlignButtonsMedium { get; set; }

        /// <summary>
        /// Gets or sets how to align the introduction on large screens
        /// </summary>
        [UmbracoProperty("Align introduction (large screens)", "AlignIntroductionLarge", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 12)]
        public string AlignIntroductionLarge { get; set; }

        /// <summary>
        /// Gets or sets how to align the landing navigation on large screens
        /// </summary>
        [UmbracoProperty("Align landing navigation (large screens)", "AlignLandingNavigationLarge", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 13)]
        public string AlignLandingNavigationLarge { get; set; }

        /// <summary>
        /// Gets or sets how to align the buttons on large screens
        /// </summary>
        [UmbracoProperty("Align buttons (large screens)", "AlignButtonsLarge", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 14)]
        public string AlignButtonsLarge { get; set; }

        [UmbracoProperty("Space above buttons (small screens)", "ButtonsTopMarginSmall", BuiltInUmbracoDataTypes.Textbox, sortOrder: 15, ValidationRegularExpression = "^(|[0-9]{1,4})$",
            Description = "Additional space, in pixels, for the background image to show between the heading or intro and the buttons. This will still apply at larger sizes but may be overridden by the medium and large screen settings.")]
        public string ButtonsTopMarginSmall { get; set; }

        [UmbracoProperty("Space above buttons (medium screens)", "ButtonsTopMarginMedium", BuiltInUmbracoDataTypes.Textbox, sortOrder: 16, ValidationRegularExpression = "^(|[0-9]{1,4})$",
            Description = "Additional space, in pixels, for the background image to show between the heading or intro and the buttons. This will still apply at larger sizes but may be overridden by the large screen setting.")]
        public string ButtonsTopMarginMedium { get; set; }

        [UmbracoProperty("Space above buttons (large screens)", "ButtonsTopMarginLarge", BuiltInUmbracoDataTypes.Textbox, sortOrder: 17, ValidationRegularExpression = "^(|[0-9]{1,4})$",
            Description = "Additional space, in pixels, for the background image to show between the heading or intro and the buttons on large screens.")]
        public string ButtonsTopMarginLarge { get; set; }

        [UmbracoProperty("Video height", "VideoHeight", BuiltInUmbracoDataTypes.Textbox, sortOrder: 18, ValidationRegularExpression = "^(|[0-9]{1,4})$",
            Description = "Videos are resized automatically based on an initial aspect ratio. Set the height higher or lower than 250 to change the aspect ratio.")]
        public string VideoHeight { get; set; }

        [UmbracoProperty("Content text colour", "ContentColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 19)]
        public string ContentColour { get; set; }

        [UmbracoProperty("Custom CSS (small screens)", "CssSmall", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 20,
             Description = "Custom CSS to apply to small screens such as mobiles. This will still apply at larger sizes but may be overridden by the CSS for medium and large screens.")]
        public string CssSmall { get; set; }

        [UmbracoProperty("Custom CSS (medium screens)", "CssMedium", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 21,
             Description = "Custom CSS to apply to medium screens such as tablets. This will still apply at larger sizes but may be overridden by the CSS for large screens.")]
        public string CssMedium { get; set; }

        [UmbracoProperty("Custom CSS (large screens)", "CssLarge", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 22,
            Description = "Custom CSS to apply to large screens such as laptops. This is added to and can override the CSS for small and medium screens.")]
        public string CssLarge { get; set; }
    }
}