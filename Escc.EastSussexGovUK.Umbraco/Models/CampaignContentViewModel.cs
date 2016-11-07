using System.Collections.Generic;
using System.Web;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Models
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
        public string UpperQuote { get; set; }
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
        public string LowerQuote { get; set; }
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