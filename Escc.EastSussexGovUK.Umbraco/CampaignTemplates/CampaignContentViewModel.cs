using System.Collections.Generic;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.CampaignTemplates
{
    /// <summary>
    /// Model for the campaign content Umbraco document type
    /// </summary>
    public class CampaignContentViewModel : BaseViewModel
    {
        public Image BannerImageSmall { get; set; }
        public Image BannerImageLarge { get; set; }
        public IHtmlString ContentPart1 { get; set; }
        public IHtmlString ContentPart2 { get; set; }
        public IHtmlString ContentPart3 { get; set; }
        public IHtmlString ContentPart4 { get; set; }
        public IHtmlString ContentPart5 { get; set; }
        public IHtmlString ContentPart6 { get; set; }

        /// <summary>
        /// Gets or sets a pull quote which appears in the upper half of the page
        /// </summary>
        public string UpperQuote { get; set; }

        /// <summary>
        /// Gets or sets the an image which appears in the upper half of the page.
        /// </summary>
        public Image UpperImage { get; set; }

        public string CentralQuote { get; set; }

        /// <summary>
        /// Gets or sets the image which illustrates the central quote; typically the speaker.
        /// </summary>
        /// <value>
        /// The central quote image.
        /// </value>
        public Image CentralQuoteImage { get; set; }

        /// <summary>
        /// Gets or sets whetther the central pull quote image is a cutout of a person.
        /// </summary>
        /// <value>
        /// <c>true</c> if the central pull quote image is a cutout; <c>false</c> otherwise.
        /// </value>
        public bool CentralQuoteImageIsCutout { get; set; }

        /// <summary>
        /// Gets or sets a pull quote which appears in the lower half of the page.
        /// </summary>
        /// <value>
        /// The lower quote.
        /// </value>
        public string LowerQuote { get; set; }

        /// <summary>
        /// Gets or sets the an image which appears in the lower half of the page.
        /// </summary>
        public Image LowerImage { get; set; }

        public IList<GuideNavigationLink> CampaignPages { get; set; } = new List<GuideNavigationLink>();
        /// <summary>
        /// Gets or sets the upper and lower pull quote text colour.
        /// </summary>
        public string PullQuoteTextColour { get; set; }
        /// <summary>
        /// Gets or sets the upper and lower pull quote background colour.
        /// </summary>
        public string PullQuoteBackgroundColour { get; set; }
        /// <summary>
        /// Gets or sets the upper and lower pull quote quotation marks colour.
        /// </summary>
        public string PullQuoteQuotationMarksColour { get; set; }
        /// <summary>
        /// Gets or sets the text colour for the central quote.
        /// </summary>
        public string CentralQuoteTextColour { get; set; }
        /// <summary>
        /// Gets or sets the background colour for the central quote.
        /// </summary>
        public string CentralQuoteBackgroundColour { get; set; }
        /// <summary>
        /// Gets or sets the final pull quote which ends the content.
        /// </summary>
        public string FinalQuote { get; set; }
        /// <summary>
        /// Gets or sets the name of the speaker for the final pull quote
        /// </summary>
        public string FinalQuoteAttribution { get; set; }
        /// <summary>
        /// Gets or sets the image of the speaker for the final pull quote
        /// </summary>
        public Image FinalQuoteImage { get; set; }
        /// <summary>
        /// Gets or sets the text colour for the final pull quote.
        /// </summary>
        public string FinalQuoteTextColour { get; set; }

        /// <summary>
        /// Gets or sets whether to set pull quotes in bold type
        /// </summary>
        /// <value>
        ///   <c>true</c> if quotes in bold; otherwise, <c>false</c>.
        /// </value>
        public bool QuotesInBold { get; set; }

        /// <summary>
        /// Gets or sets a value whether to set pull quotes in italic type
        /// </summary>
        /// <value>
        ///   <c>true</c> if quotes in italic; otherwise, <c>false</c>.
        /// </value>
        public bool QuotesInItalic { get; set; }

        /// <summary>
        /// Gets or sets the font family to use for all pull quotes, as a Google Fonts URL segment.
        /// </summary>
        /// <example>Cormorant+Garamond:400,400i</example>
        public string QuoteFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the custom CSS for small screens and above.
        /// </summary>
        /// <value>
        /// The custom CSS small screen.
        /// </value>
        public IHtmlString CustomCssSmallScreen { get; set; }
        /// <summary>
        /// Gets or sets the custom CSS for medium screens and above, overriding the small screen CSS.
        /// </summary>
        /// <value>
        /// The custom CSS medium screen.
        /// </value>
        public IHtmlString CustomCssMediumScreen { get; set; }
        /// <summary>
        /// Gets or sets the custom CSS for large screens, overriding the small and medium screen CSS.
        /// </summary>
        /// <value>
        /// The custom CSS large screen.
        /// </value>
        public IHtmlString CustomCssLargeScreen { get; set; }

        /// <summary>
        /// Gets or sets a custom height for videos on this page
        /// </summary>
        /// <value>
        /// The height of the video.
        /// </value>
        public int? VideoWidth { get; set; }

        /// <summary>
        /// Gets or sets a custom height for videos on this page
        /// </summary>
        /// <value>
        /// The height of the video.
        /// </value>
        public int? VideoHeight { get; set; }
    }
}