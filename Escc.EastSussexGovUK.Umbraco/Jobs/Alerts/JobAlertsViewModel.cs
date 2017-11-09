using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// A view model for configuring the job alerts feature
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.BaseJobsViewModel" />
    public class JobAlertsViewModel : JobsSearchViewModel
    {
        /// <summary>
        /// Gets or sets the alert to cancel or change
        /// </summary>
        /// <value>
        /// The alert.
        /// </value>
        public JobAlert Alert { get; set; }

        /// <summary>
        /// Gets the text of the submit button.
        /// </summary>
        public override string SubmitButtonText { get { return "Save alert"; } }
    }
}