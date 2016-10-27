namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// View model for the landing template
    /// </summary>
    public class LandingViewModel : BaseViewModel
    {
        public LandingNavigationViewModel Navigation { get; set; } = new LandingNavigationViewModel();
    }
}