using Escc.EastSussexGovUK.Umbraco.Models;

namespace Escc.EastSussexGovUK.Umbraco.Landing
{
    /// <summary>
    /// View model for the landing template
    /// </summary>
    public class LandingViewModel : BaseViewModel
    {
        public LandingNavigationViewModel Navigation { get; set; } = new LandingNavigationViewModel();
    }
}