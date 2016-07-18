using Escc.EastSussexGovUK.UmbracoViews.ViewModels;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// View model for the landing template
    /// </summary>
    public class LandingViewModel : BaseViewModelWithInheritedContent
    {
        public LandingNavigationViewModel Navigation { get; set; } = new LandingNavigationViewModel();
    }
}