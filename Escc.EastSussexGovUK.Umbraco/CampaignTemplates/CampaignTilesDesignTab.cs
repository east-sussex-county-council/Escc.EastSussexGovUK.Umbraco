using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.CampaignTemplates
{
    /// <summary>
    /// Design tab for the campaign tiles document type
    /// </summary>
    public class CampaignTilesDesignTab : TabBase
    {
        [UmbracoProperty("Banner image (small screens)", "BannerImageSmall", BuiltInUmbracoDataTypes.MediaPicker, sortOrder:1,
            Description="Banner image at the top of the page on small screens such as mobiles and tablets. 75px high, and tiles horizontally up to 802px wide.")]
        public string BannerImageSmall { get; set; }

        [UmbracoProperty("Banner image (large screens)", "BannerImageLarge", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 2,
            Description = "Banner image at the top of the page on large screens like laptops. 290px high, and tiles horizontally.")]
        public string BannerImageLarge { get; set; }

        [UmbracoProperty("Tiles: font family", "TileFontFamily", BuiltInUmbracoDataTypes.Textbox, sortOrder: 8,
            Description = "Select a font family from https://fonts.google.com and paste the name of the embedded version, eg 'Cormorant+Garamond:400,400i'")]
        public string TileFontFamily { get; set; }

        [UmbracoProperty("Tile title: text colour", "TileTitleColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 9,
            Description = "Must contrast with a white background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string TileTitleColour { get; set; }

        [UmbracoProperty("Tile description: text colour", "TileDescriptionColour", ColourPickerDataType.PropertyEditor, ColourPickerDataType.DataTypeName, sortOrder: 10,
            Description = "Must contrast with a white background to meet legal accessibility standards. Check at http://tinyurl.com/wcag-contrast")]
        public string TileDescriptionColour { get; set; }

        [UmbracoProperty("Custom CSS (small screens)", "CssSmall", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 13,
             Description = "Custom CSS to apply to small screens such as mobiles. This will still apply at larger sizes but may be overridden by the CSS for medium and large screens.")]
        public string CssSmall { get; set; }

        [UmbracoProperty("Custom CSS (medium screens)", "CssMedium", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 14,
             Description = "Custom CSS to apply to medium screens such as tablets. This will still apply at larger sizes but may be overridden by the CSS for large screens.")]
        public string CssMedium { get; set; }

        [UmbracoProperty("Custom CSS (large screens)", "CssLarge", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 15,
        Description = "Custom CSS to apply to large screens such as laptops. This is added to and can override the CSS for small and medium screens.")]
        public string CssLarge { get; set; }
    }
}