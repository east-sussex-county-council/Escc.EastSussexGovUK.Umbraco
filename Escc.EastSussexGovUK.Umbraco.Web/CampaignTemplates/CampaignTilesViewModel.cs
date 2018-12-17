using System.Collections.Generic;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.Umbraco.PropertyTypes;
using Escc.EastSussexGovUK.Umbraco.Web.Guide;

namespace Escc.EastSussexGovUK.Umbraco.Web.CampaignTemplates
{
    /// <summary>
    /// Model for the campaign tiles Umbraco document type
    /// </summary>
    public class CampaignTilesViewModel : BaseViewModel
    {
        public Image BannerImageSmall { get; set; }
        public Image BannerImageLarge { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        public IList<CampaignTile> Tiles { get; set; } = new List<CampaignTile>();

        public IList<GuideNavigationLink> CampaignPages { get; set; } = new List<GuideNavigationLink>();

        /// <summary>
        /// Gets or sets the font family to use for the text on the tiles, as a Google Fonts URL segment.
        /// </summary>
        /// <example>Cormorant+Garamond:400,400i</example>
        public string TileFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the text colour for the tile titles.
        /// </summary>
        public string TileTitleTextColour { get; set; }

        /// <summary>
        /// Gets or sets the text colour for the tile descriptions.
        /// </summary>
        public string TileDescriptionsTextColour { get; set; }

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
    }
}