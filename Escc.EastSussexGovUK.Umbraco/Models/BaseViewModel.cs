using System;
using Escc.Umbraco.ContentExperiments;
using EsccWebTeam.EastSussexGovUK.MasterPages;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// Base class for common properties to be available to all view models used on www.eastsussex.gov.uk
    /// </summary>
    public abstract class BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        protected BaseViewModel()
        {
            IsPublicView = true;
            EsccWebsiteSkin = new DefaultSkin();
        }

        /// <summary>
        /// Gets or sets the public URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the page title
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the page description.
        /// </summary>
        public string PageDescription { get; set; }

        /// <summary>
        /// Gets or sets the type of item being represented by the page
        /// </summary>
        public string PageType { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript for a Google Analytics content experiment
        /// </summary>
        public ContentExperimentPageSettings ContentExperimentPageSettings { get; set; }

        /// <summary>
        /// Gets or sets whether the current view is a publicly-visible page
        /// </summary>
        public bool IsPublicView { get; set; }

        /// <summary>
        /// Gets or sets the current master page or layout
        /// </summary>
        public EsccWebsiteView EsccWebsiteView { get; set; }

        /// <summary>
        /// Gets or sets the skin applied to content between the header and footer
        /// </summary>
        public IEsccWebsiteSkin EsccWebsiteSkin { get; set; }

        /// <summary>
        /// The identifier for the page in the content management system
        /// </summary>
        public string PageId { get; set; }
    }
}
