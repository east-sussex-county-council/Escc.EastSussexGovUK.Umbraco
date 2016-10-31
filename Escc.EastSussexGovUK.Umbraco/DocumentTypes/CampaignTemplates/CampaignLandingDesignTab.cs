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
        [UmbracoProperty("Background image (small screens)", "BackgroundSmall", BuiltInUmbracoDataTypes.MediaPicker, sortOrder:1,
            Description="Background image for small screens such as mobiles. Will be used on medium and large screens too unless another image is chosen below.")]
        public string BackgroundImageSmall { get; set; }

        [UmbracoProperty("Background image (medium screens)", "BackgroundMedium", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 2,
            Description = "Background image for medium screens such as tablets, if different from the small screen background. Will be used on large screens too unless another image is chosen below.")]
        public string BackgroundImageMedium { get; set; }

        [UmbracoProperty("Background image (large screens)", "BackgroundLarge", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 3,
            Description = "Background image for large screens such as laptops.")]
        public string BackgroundImageLarge { get; set; }

        [UmbracoProperty("Arrange buttons horizontally on medium screens", "ButtonsHorizontalAtMedium", BuiltInUmbracoDataTypes.Boolean, sortOrder: 4, 
            Description="Buttons are stacked on small screens and arranged horizontally on large screens, but you can choose what happens at medium sizes.")]
        public string ButtonsHorizontalAtMedium { get; set; }

        [UmbracoProperty("Custom CSS (small screens)", "CssSmall", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 6,
             Description="Custom CSS to apply to small screens such as mobiles. This will still apply at larger sizes but may be overridden by the CSS for medium and large screens.")]
        public string CssSmall { get; set; }

        [UmbracoProperty("Custom CSS (medium screens)", "CssMedium", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 7,
             Description = "Custom CSS to apply to medium screens such as tablets. This will still apply at larger sizes but may be overridden by the CSS for large screens.")]
        public string CssMedium { get; set; }

        [UmbracoProperty("Custom CSS (large screens)", "CssLarge", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 8,
            Description="Custom CSS to apply to large screens such as laptops. This is added to and can override the CSS for small and medium screens.")]
        public string CssLarge { get; set; }
    }
}