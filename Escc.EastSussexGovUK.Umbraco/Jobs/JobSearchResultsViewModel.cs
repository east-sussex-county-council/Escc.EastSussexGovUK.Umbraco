using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;
using X.PagedList;

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
        /// Gets or sets the job detail page.
        /// </summary>
        public HtmlLink JobDetailPage { get; set; }

        /// <summary>
        /// Gets or sets the jobs search page.
        /// </summary>
        public HtmlLink JobsSearchPage { get; set; }

        /// <summary>
        /// Gets or sets the results URL where the data comes from.
        /// </summary>
        /// <value>
        /// The results URL.
        /// </value>
        public TalentLinkUrl ResultsUrl { get; set; }

        /// <summary>
        /// Gets the jobs to display
        /// </summary>
        public IPagedList<Job> Jobs { get; set; }

        /// <summary>
        /// Gets or sets the query that led to these results
        /// </summary>
        public JobSearchQuery Query { get; set; }
    }
}