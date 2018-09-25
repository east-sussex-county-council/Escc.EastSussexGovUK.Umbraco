using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Landing
{
    public class LandingNavigationViewModel
    {
        /// <summary>
        /// Gets or sets the sections of links within the landing navigation.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        public IList<LandingSectionViewModel> Sections { get; set; }

        /// <summary>
        /// Gets or sets the landing navigation layout.
        /// </summary>
        /// <value>
        /// The landing layout.
        /// </value>
        public LandingNavigationLayout LandingNavigationLayout { get; set; }
    }
}