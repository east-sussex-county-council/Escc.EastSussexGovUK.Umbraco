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
    public class JobSearchResultsViewModel : BaseJobsViewModel
    {
        /// <summary>
        /// Gets or sets the job alerts page
        /// </summary>
        public HtmlLink JobAlertsPage { get; set; }

        /// <summary>
        /// Gets or sets the job detail page.
        /// </summary>
        public HtmlLink JobDetailPage { get; set; }

        /// <summary>
        /// Gets or sets the name of the Examine searcher for the jobs index
        /// </summary>
        public string ExamineSearcher { get; set; }

        /// <summary>
        /// Gets or sets the jobs search page.
        /// </summary>
        public HtmlLink JobsSearchPage { get; set; }

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