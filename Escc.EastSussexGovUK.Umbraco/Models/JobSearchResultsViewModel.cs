using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// The model for the search results page of the jobs service
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobSearchResultsViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the logo for the jobs service
        /// </summary>
        public Image JobsLogo { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header
        /// </summary>
        public Image HeaderBackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the login page
        /// </summary>
        public HtmlLink LoginPage { get; set; }

        /// <summary>
        /// Gets or sets the URL of the script to embed the component in the page.
        /// </summary>
        /// <value>
        /// The script URL.
        /// </value>
        public Uri ResultsScriptUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL to link to the component on a separate page.
        /// </summary>
        /// <value>
        /// The link URL.
        /// </value>
        public Uri ResultsLinkUrl { get; set; }

        /// <summary>
        /// Gets or sets the URLs the buttons should link to
        /// </summary>
        public IList<HtmlLink> ButtonNavigation { get; set; }

        /// <summary>
        /// Gets or sets the images to link using <see cref="ButtonNavigation"/>
        /// </summary>
        public IList<Image> ButtonImages { get; set; }
    }
}