using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// The model for the start page of the jobs service
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobsHomeViewModel : BaseViewModel
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
        /// Gets or sets the search results page
        /// </summary>
        public HtmlLink SearchResultsPage { get; set; }

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