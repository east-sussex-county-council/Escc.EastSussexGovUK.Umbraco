using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// An alert which will be sent on a schedule determined by the <see cref="Frequency" /> when new jobs are available matching the <see cref="Query" />
    /// </summary>
    public class JobAlert
    {
        /// <summary>
        /// Gets or sets the query the alert is based on.
        /// </summary>
        public JobSearchQuery Query { get; set; }

        /// <summary>
        /// Gets or sets the email address the alert will be sent to.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the frequency, in days, with which the alert will be sent.
        /// </summary>
        public int Frequency { get; set; } = 1;

        /// <summary>
        /// Gets the new jobs which have been matched for the alert, based on the <see cref="Query"/>
        /// </summary>
        public IList<Job> MatchingJobs { get; private set; } = new List<Job>();

        /// <summary>
        /// Gets or sets the unique identifier for this alert.
        /// </summary>
        public string AlertId { get; set; }

        /// <summary>
        /// Gets or sets the set of jobs this alert applies to
        /// </summary>
        public JobsSet JobsSet { get; set; }
    }
}