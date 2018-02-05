using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.CampaignTemplates
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

        [UmbracoProperty("Breadcrumb text colour", "BreadcrumbColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 7,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string BreadcrumbColour { get; set; }

        [UmbracoProperty("Heading text colour", "HeadingColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 8,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string HeadingColour { get; set; }

        [UmbracoProperty("Introduction text colour", "IntroductionColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 9,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string IntroductionColour { get; set; }
        
        /// <summary>
        /// Gets or sets how to align the introduction on medium screens
        /// </summary>
        [UmbracoProperty("Align introduction (medium screens)", "AlignIntroductionMedium", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 10)]
        public string AlignIntroductionMedium { get; set; }

        /// <summary>
        /// Gets or sets how to align the introduction on large screens
        /// </summary>
        [UmbracoProperty("Align introduction (large screens)", "AlignIntroductionLarge", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 11)]
        public string AlignIntroductionLarge { get; set; }


        /// <summary>
        /// Gets or sets how to align the landing navigation on medium screens
        /// </summary>
        [UmbracoProperty("Align landing navigation (medium screens)", "AlignLandingNavigationMedium", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 12)]
        public string AlignLandingNavigationMedium { get; set; }

        /// <summary>
        /// Gets or sets how to align the landing navigation on large screens
        /// </summary>
        [UmbracoProperty("Align landing navigation (large screens)", "AlignLandingNavigationLarge", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 13)]
        public string AlignLandingNavigationLarge { get; set; }

        [UmbracoProperty("Landing navigation background colour", "LandingNavigationBackgroundColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 14,
    Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string LandingNavigationBackgroundColour { get; set; }

        [UmbracoProperty("Landing navigation text colour", "LandingNavigationTextColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 15,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string LandingNavigationTextColour { get; set; }

        /// <summary>
        /// Gets or sets how to align the buttons on medium screens
        /// </summary>
        [UmbracoProperty("Align buttons (medium screens)", "AlignButtonsMedium", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 16)]
        public string AlignButtonsMedium { get; set; }

        /// <summary>
        /// Gets or sets how to align the buttons on large screens
        /// </summary>
        [UmbracoProperty("Align buttons (large screens)", "AlignButtonsLarge", AlignmentDataType.PropertyEditor, AlignmentDataType.DataTypeName, sortOrder: 17)]
        public string AlignButtonsLarge { get; set; }

        [UmbracoProperty("Space above buttons (small screens)", "ButtonsTopMarginSmall", BuiltInUmbracoDataTypes.Textbox, sortOrder: 18, ValidationRegularExpression = "^(|[0-9]{1,4})$",
            Description = "Additional space, in pixels, for the background image to show between the heading or intro and the buttons. This will still apply at larger sizes but may be overridden by the medium and large screen settings.")]
        public string ButtonsTopMarginSmall { get; set; }

        [UmbracoProperty("Space above buttons (medium screens)", "ButtonsTopMarginMedium", BuiltInUmbracoDataTypes.Textbox, sortOrder: 19, ValidationRegularExpression = "^(|[0-9]{1,4})$",
            Description = "Additional space, in pixels, for the background image to show between the heading or intro and the buttons. This will still apply at larger sizes but may be overridden by the large screen setting.")]
        public string ButtonsTopMarginMedium { get; set; }

        [UmbracoProperty("Space above buttons (large screens)", "ButtonsTopMarginLarge", BuiltInUmbracoDataTypes.Textbox, sortOrder: 20, ValidationRegularExpression = "^(|[0-9]{1,4})$",
            Description = "Additional space, in pixels, for the background image to show between the heading or intro and the buttons on large screens.")]
        public string ButtonsTopMarginLarge { get; set; }

        [UmbracoProperty("Button 1 background colour", "Button1BackgroundColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 21,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string Button1BackgroundColour { get; set; }

        [UmbracoProperty("Button 1 text colour", "Button1TextColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 22,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string Button1TextColour { get; set; }

        [UmbracoProperty("Button 2 background colour", "Button2BackgroundColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 23,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string Button2BackgroundColour { get; set; }

        [UmbracoProperty("Button 2 text colour", "Button2TextColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 24,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string Button2TextColour { get; set; }

        [UmbracoProperty("Button 3 background colour", "Button3BackgroundColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 25,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string Button3BackgroundColour { get; set; }

        [UmbracoProperty("Button 3 text colour", "Button3TextColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 26,
            Description = "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string Button3TextColour { get; set; }

        [UmbracoProperty("Video height", "VideoHeight", BuiltInUmbracoDataTypes.Textbox, sortOrder: 27, ValidationRegularExpression = "^(|[0-9]{1,4})$",
            Description = "Videos are resized automatically based on an initial aspect ratio. Set the height higher or lower than 250 to change the aspect ratio.")]
        public string VideoHeight { get; set; }

        [UmbracoProperty("Content text colour", "ContentColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 28, 
            Description= "Text must contrast with its background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string ContentColour { get; set; }

        [UmbracoProperty("Share links colour", "ShareStyle", ShareStyleDataType.PropertyEditor, ShareStyleDataType.DataTypeName, sortOrder: 29,
            Description="Sets the colour of the icons and text for the social media and comment links at the end of the page. Must contrast with the background colour.")]
        public string ShareStyle { get; set; }

        [UmbracoProperty("Custom CSS (small screens)", "CssSmall", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 30,
             Description = "Custom CSS to apply to small screens such as mobiles. This will still apply at larger sizes but may be overridden by the CSS for medium and large screens.")]
        public string CssSmall { get; set; }

        [UmbracoProperty("Custom CSS (medium screens)", "CssMedium", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 31,
             Description = "Custom CSS to apply to medium screens such as tablets. This will still apply at larger sizes but may be overridden by the CSS for large screens.")]
        public string CssMedium { get; set; }

        [UmbracoProperty("Custom CSS (large screens)", "CssLarge", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 32,
            Description = "Custom CSS to apply to large screens such as laptops. This is added to and can override the CSS for small and medium screens.")]
        public string CssLarge { get; set; }
    }
}