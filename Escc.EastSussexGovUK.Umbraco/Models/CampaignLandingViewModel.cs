using System.Collections.Generic;
using System.Web;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// A model for the social work campaign landing page view
    /// </summary>
    public class CampaignLandingViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignLandingViewModel"/> class.
        /// </summary>
        public CampaignLandingViewModel()
        {
            ButtonTargets = new List<HtmlLink>();    
            ButtonDescriptions = new List<string>();
        }

        public IHtmlString Introduction { get; set; }
        public LandingNavigationViewModel LandingNavigation { get; set;} = new LandingNavigationViewModel();
        public IList<HtmlLink> ButtonTargets { get; set; }
        public IList<string> ButtonDescriptions { get; set; }

        /// <summary>
        /// Gets or sets whether buttons should be stacked or horizontal at medium size. Buttons are always horizontal at large sizes.
        /// </summary>
        /// <value>
        /// <c>true</c> if buttons horizontal at medium; <c>false</c> if stacked.
        /// </value>
        public bool ButtonsHorizontalAtMedium { get; set; }
        public IHtmlString Content { get; set; }
        public IHtmlString CustomCssSmallScreen { get; set; }
        public IHtmlString CustomCssMediumScreen { get; set; }
        public IHtmlString CustomCssLargeScreen { get; set; }

        /// <summary>
        /// Gets or sets the background colour of the page.
        /// </summary>
        /// <value>
        /// The background colour.
        /// </value>
        public string BackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the background image which shows from small screens until it's overridden.
        /// </summary>
        /// <value>
        /// The background image.
        /// </value>
        public Image BackgroundImageSmall { get; set; }

        /// <summary>
        /// Gets or sets the background image which overrides the small screen background.
        /// </summary>
        /// <value>
        /// The background image.
        /// </value>
        public Image BackgroundImageMedium{ get; set; }
        
        /// <summary>
        /// Gets or sets the background image which overrides the small or medium background
        /// </summary>
        /// <value>
        /// The background image large.
        /// </value>
        public Image BackgroundImageLarge{ get; set; }

        /// <summary>
        /// Gets or sets a value whether to wrap the background image horizontally
        /// </summary>
        /// <value>
        ///   <c>true</c> to wrap the background image horizontally; otherwise, <c>false</c>.
        /// </value>
        public bool BackgroundImageWrapsHorizontally { get; set; }

        /// <summary>
        /// Gets or sets a value whether to wrap the background image vertically
        /// </summary>
        /// <value>
        ///   <c>true</c> to wrap the background image vertically; otherwise, <c>false</c>.
        /// </value>
        public bool BackgroundImageWrapsVertically { get; set; }
    }
}