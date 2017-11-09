using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// The model for a job advert document type of the jobs service
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.BaseJobsViewModel" />
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobAdvertViewModel : BaseJobsViewModel
    {
        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        public Job Job { get; set; }

        /// <summary>
        /// Gets or sets the set of jobs the advert belongs to
        /// </summary>
        public JobsSet JobsSet { get; set; }

        /// <summary>
        /// Gets or sets jobs which may be similar to this one.
        /// </summary>
        public List<Job> SimilarJobs { get; set; } = new List<Job>();

        /// <summary>
        /// Gets or sets the search results page for use when a job has closed
        /// </summary>
        public HtmlLink SearchResultsPageForClosedJobs { get; set; }
    }
}