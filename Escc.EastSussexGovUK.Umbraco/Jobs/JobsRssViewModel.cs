using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// The model for the jobs RSS feed
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobsRssViewModel : RssViewModel<Job>
    {
        /// <summary>
        /// Gets or sets the job detail page.
        /// </summary>
        public HtmlLink JobAdvertPage { get; set; }

        /// <summary>
        /// Gets or sets the jobs to include in the feed
        /// </summary>
        public JobsSet JobsSet { get; set; }

        /// <summary>
        /// Gets or sets the query that led to these results
        /// </summary>
        public JobSearchQuery Query { get; set; }
    }
}