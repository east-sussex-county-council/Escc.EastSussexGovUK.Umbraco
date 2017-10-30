using System;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using Escc.Umbraco.ContentExperiments;
using Escc.EastSussexGovUK.Umbraco.Ratings;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// Base class for common properties to be available to all view models used on www.eastsussex.gov.uk
    /// </summary>
    public abstract class BaseViewModel : Mvc.BaseViewModel
    {
        /// <summary>
        /// Gets or sets the type of item being represented by the page
        /// </summary>
        public string PageType { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript for a Google Analytics content experiment
        /// </summary>
        public ContentExperimentPageSettings ContentExperimentPageSettings { get; set; }

        /// <summary>
        /// Gets or sets the settings for a page rating service
        /// </summary>
        public RatingSettings RatingSettings { get; set; }

        /// <summary>
        /// Gets or sets the name of a skin to apply.
        /// </summary>
        public string SkinToApply { get; set; }
    }
}
