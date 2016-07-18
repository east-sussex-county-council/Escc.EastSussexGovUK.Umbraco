using System.Collections.Generic;
using Escc.EastSussexGovUK.UmbracoViews.ViewModels;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// A complete guide to a process
    /// </summary>
    public class GuideViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the steps in the guide.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        public IEnumerable<GuideStepViewModel> Steps { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuideViewModel"/> class.
        /// </summary>
        public GuideViewModel()
        {
            Steps = new List<GuideStepViewModel>();
        }
    }
}