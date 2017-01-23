using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// The model for a job advert document type of the jobs service
    /// </summary>
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
        /// Gets or sets the job advert URL where the data comes from.
        /// </summary>
        /// <value>
        /// The job advert URL.
        /// </value>
        public TalentLinkUrl JobAdvertUrl { get; set; }
    }
}