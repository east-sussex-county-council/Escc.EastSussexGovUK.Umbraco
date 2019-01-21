using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Web.Jobs
{
    /// <summary>
    /// Base view model for pages in the jobs section
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public abstract class BaseJobsViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the logo for the jobs service
        /// </summary>
        public Image JobsLogo { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header on small screens
        /// </summary>
        public Image HeaderBackgroundImageSmall { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header on medium screens
        /// </summary>
        public Image HeaderBackgroundImageMedium { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header on large screens
        /// </summary>
        public Image HeaderBackgroundImageLarge { get; set; }

        /// <summary>
        /// Gets or sets the header background image caption.
        /// </summary>
        /// <value>
        /// The header background image caption.
        /// </value>
        public string HeaderBackgroundImageCaption { get; set; }
        
        /// <summary>
        /// Gets or sets the home page for the jobs service
        /// </summary>
        public HtmlLink JobsHomePage { get; set; }

        /// <summary>
        /// Gets or sets the login page
        /// </summary>
        [Obsolete("Use LoginUrl")]
        public HtmlLink LoginPage { get; set; }

        /// <summary>
        /// Gets or sets the login URL
        /// </summary>
        public Uri LoginUrl { get; set; }

        /// <summary>
        /// Gets or sets the search page
        /// </summary>
        public HtmlLink SearchPage { get; set; }

        /// <summary>
        /// Gets or sets the search results page
        /// </summary>
        public HtmlLink SearchResultsPageForHeader { get; set; }
    }
}