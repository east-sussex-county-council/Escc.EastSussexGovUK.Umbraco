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
        public string UpperQuote { get; set; }
        public string CentralQuote { get; set; }
        public Image CentralQuoteImage { get; set; }
        public string LowerQuote { get; set; }
        public string FinalQuote { get; set; }
        public Image FinalQuoteImage { get; set; }
        public IList<GuideNavigationLink> CampaignPages { get; set; } = new List<GuideNavigationLink>();
        public string PullQuoteBackgroundColour { get; set; }
        public string PullQuoteQuotationMarksColour { get; set; }
        public string CentralQuoteBackgroundColour { get; set; }
        public string FinalQuoteTextColour { get; set; }
        public IHtmlString CustomCssLargeScreen { get; set; }
    }
}