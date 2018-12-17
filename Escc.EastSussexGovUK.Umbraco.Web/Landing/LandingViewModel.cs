using Escc.EastSussexGovUK.Umbraco.Web.Models;

namespace Escc.EastSussexGovUK.Umbraco.Web.Landing
{
    /// <summary>
    /// View model for the landing template
    /// </summary>
    public class LandingViewModel : BaseViewModel
    {
        public LandingNavigationViewModel Navigation { get; set; } = new LandingNavigationViewModel();
    }
}