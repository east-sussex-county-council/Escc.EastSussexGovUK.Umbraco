using System.Collections.Generic;
using System.Web;
using Escc.EastSussexGovUK.UmbracoViews.ViewModels;
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
        public IHtmlString Content { get; set; }
        public IHtmlString CustomCssSmallScreen { get; set; }
        public IHtmlString CustomCssMediumScreen { get; set; }
        public IHtmlString CustomCssLargeScreen { get; set; }
        public Image BackgroundImageSmall { get; set; }
        public Image BackgroundImageMedium{ get; set; }
        public Image BackgroundImageLarge{ get; set; }
    }
}