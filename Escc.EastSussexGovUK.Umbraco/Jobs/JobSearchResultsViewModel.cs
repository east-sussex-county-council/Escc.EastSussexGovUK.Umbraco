﻿using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;
using Escc.NavigationControls.WebForms;

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
        public HtmlLink JobAdvertPage { get; set; }

        /// <summary>
        /// Gets or sets the set of jobs to show results for
        /// </summary>
        public JobsSet JobsSet { get; set; }

        /// <summary>
        /// Gets or sets the jobs search page.
        /// </summary>
        public HtmlLink JobsSearchPage { get; set; }

        /// <summary>
        /// Gets or sets the privacy notice page for how jobs data is used.
        /// </summary>
        public HtmlLink JobsPrivacyPage { get; set; }

        /// <summary>
        /// Gets the jobs to display
        /// </summary>
        public IList<Job> Jobs { get; set; }

        /// <summary>
        /// Gets or sets the query that led to these results
        /// </summary>
        public JobSearchQuery Query { get; set; }

        /// <summary>
        /// Gets the configuration for paging the results
        /// </summary>
        public PagingController Paging { get; private set; } = new PagingController();
    }
}