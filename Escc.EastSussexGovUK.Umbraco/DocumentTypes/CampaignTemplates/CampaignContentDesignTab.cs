using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates
{
    /// <summary>
    /// Design tab for the campaign content document type
    /// </summary>
    public class CampaignContentDesignTab : TabBase
    {
        [UmbracoProperty("Banner image (small screens)", "BannerImageSmall", BuiltInUmbracoDataTypes.MediaPicker, sortOrder:1,
            Description="Banner image at the top of the page on small screens such as mobiles and tablets. 75px high, and tiles horizontally up to 802px wide.")]
        public string BannerImageSmall { get; set; }

        [UmbracoProperty("Banner image (large screens)", "BannerImageLarge", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 2,
            Description = "Banner image at the top of the page on large screens like laptops. 290px high, and tiles horizontally.")]
        public string BannerImageLarge { get; set; }

        [UmbracoProperty("Upper/lower pull quote: background colour", "PullQuoteBackground", CampaignQuoteColourDataType.PropertyEditor, CampaignQuoteColourDataType.DataTypeName, sortOrder: 3,
            Description = "Must contrast with white text and the selected colour for quotation marks.")]
        public string PullQuoteBackground { get; set; }

        [UmbracoProperty("Upper/lower pull quote: quotation marks colour", "PullQuoteMarks", CampaignQuoteColourDataType.PropertyEditor, CampaignQuoteColourDataType.DataTypeName, sortOrder: 4,
            Description = "Must contrast with white text and the selected background colour.")]
        public string PullQuoteQuotations { get; set; }

        [UmbracoProperty("Central pull quote: background colour", "CentralQuoteBackground", CampaignQuoteColourDataType.PropertyEditor, CampaignQuoteColourDataType.DataTypeName, sortOrder: 5,
            Description="Must contrast with white text.")]
        public string CentralQuoteBackground { get; set; }

        [UmbracoProperty("Central pull quote image is a cutout of a person", "CentralQuoteImageIsCutout", BuiltInUmbracoDataTypes.Boolean, sortOrder: 6,
Description = "This will add a line below the image which shows up if the quote is taller than the image.")]
        public string CentralQuoteImageIsCutout { get; set; }

        [UmbracoProperty("Final pull quote: text colour", "FinalQuoteColour", CampaignQuoteColourDataType.PropertyEditor, CampaignQuoteColourDataType.DataTypeName, sortOrder: 7,
            Description = "Must contrast with a white background.")]
        public string FinalQuoteColour { get; set; }

        [UmbracoProperty("Custom CSS (large screens)", "CssLarge", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 9,
        Description = "Custom CSS to apply to large screens such as laptops. This is added to and can override the CSS for small and medium screens.")]
        public string CssLarge { get; set; }

        [UmbracoProperty("Video width", "VideoWidth", BuiltInUmbracoDataTypes.Textbox, sortOrder: 11, ValidationRegularExpression = "^(|[0-9]{1,4})$",
    Description = "Change the default width of videos on this page")]
        public string VideoWidth { get; set; }

        [UmbracoProperty("Video height", "VideoHeight", BuiltInUmbracoDataTypes.Textbox, sortOrder: 12, ValidationRegularExpression = "^(|[0-9]{1,4})$",
    Description = "Change the default height of videos on this page.")]
        public string VideoHeight { get; set; }
    }
}