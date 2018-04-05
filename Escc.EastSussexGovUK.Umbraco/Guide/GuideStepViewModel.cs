using System;
using System.Collections.Generic;
using System.Web;
using Escc.Umbraco.PropertyTypes;
using Escc.EastSussexGovUK.Umbraco.Models;

namespace Escc.EastSussexGovUK.Umbraco.Guide
{
    /// <summary>
    /// View model for a Guide Step view
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class GuideStepViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuideStepViewModel"/> class.
        /// </summary>
        public GuideStepViewModel()
        {
            RelatedLinksGroups = new List<LandingSectionViewModel>();
            PartnerImages = new List<Image>();
        }

        /// <summary>
        /// Gets or sets the URL of the guide containing this guide step.
        /// </summary>
        /// <value>
        /// The guide URL.
        /// </value>
        public Uri GuideUrl { get; set; }

        /// <summary>
        /// Gets or sets the title of the guide containing this guide step.
        /// </summary>
        /// <value>
        /// The guide title.
        /// </value>
        public string GuideTitle { get; set; }
        public IList<GuideNavigationLink> Steps { get; set; }
        public IHtmlString StepContent { get; set; }
        public IList<LandingSectionViewModel> RelatedLinksGroups { get; private set; }
        public IList<Image> PartnerImages { get; private set; }

        /// <summary>
        /// Gets or sets whether steps should be followed from 1 to x, rather than in any order
        /// </summary>
        /// <value>
        ///   <c>true</c> if steps have an order; otherwise, <c>false</c>.
        /// </value>
        public bool StepsHaveAnOrder { get; set; }
    }
}