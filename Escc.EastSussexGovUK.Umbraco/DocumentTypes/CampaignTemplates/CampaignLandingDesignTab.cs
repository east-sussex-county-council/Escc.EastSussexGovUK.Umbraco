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

        [UmbracoProperty("Arrange buttons horizontally on medium screens", "ButtonsHorizontalAtMedium", BuiltInUmbracoDataTypes.Boolean, sortOrder: 7, 
            Description="Buttons are stacked on small screens and arranged horizontally on large screens, but you can choose what happens at medium sizes.")]
        public string ButtonsHorizontalAtMedium { get; set; }

        [UmbracoProperty("Custom CSS (small screens)", "CssSmall", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 8,
             Description="Custom CSS to apply to small screens such as mobiles. This will still apply at larger sizes but may be overridden by the CSS for medium and large screens.")]
        public string CssSmall { get; set; }

        [UmbracoProperty("Custom CSS (medium screens)", "CssMedium", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 9,
             Description = "Custom CSS to apply to medium screens such as tablets. This will still apply at larger sizes but may be overridden by the CSS for large screens.")]
        public string CssMedium { get; set; }

        [UmbracoProperty("Custom CSS (large screens)", "CssLarge", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 10,
            Description="Custom CSS to apply to large screens such as laptops. This is added to and can override the CSS for small and medium screens.")]
        public string CssLarge { get; set; }

        [UmbracoProperty("Video height", "VideoHeight", BuiltInUmbracoDataTypes.Textbox, sortOrder: 11, ValidationRegularExpression = "^(|[0-9]{1,4})$",
            Description = "Videos are resized automatically based on an initial aspect ratio. Set the height higher or lower than 250 to change the aspect ratio.")]
        public string VideoHeight { get; set; }

    }
}