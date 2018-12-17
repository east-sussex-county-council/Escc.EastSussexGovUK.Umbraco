using Escc.EastSussexGovUK.Umbraco.Web.Models;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Web.Guide
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


        /// <summary>
        /// Gets or sets whether steps should be followed from 1 to x, rather than in any order
        /// </summary>
        /// <value>
        ///   <c>true</c> if steps have an order; otherwise, <c>false</c>.
        /// </value>
        public bool StepsHaveAnOrder { get; set; }
    }
}