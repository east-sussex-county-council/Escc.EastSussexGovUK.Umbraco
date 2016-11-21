using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
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
        /// Gets or sets the home page for the jobs service
        /// </summary>
        public HtmlLink JobsHomePage { get; set; }

        /// <summary>
        /// Gets or sets the login page
        /// </summary>
        public HtmlLink LoginPage { get; set; }

        /// <summary>
        /// Gets or sets the job alerts page
        /// </summary>
        public HtmlLink JobAlertsPage { get; set; }

        /// <summary>
        /// Gets or sets the URL of the script to embed the component in the page.
        /// </summary>
        /// <value>
        /// The script URL.
        /// </value>
        public TalentLinkUrl ResultsUrl { get; set; }

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