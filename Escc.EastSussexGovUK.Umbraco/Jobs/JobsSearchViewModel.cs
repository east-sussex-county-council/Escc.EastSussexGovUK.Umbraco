using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// View model for the search page of the jobs section
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobsSearchViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the logo for the jobs service
        /// </summary>
        public Image JobsLogo { get; set; }

        /// <summary>
        /// Gets or sets the home page for the jobs service
        /// </summary>
        public HtmlLink JobsHomePage { get; set; }

        /// <summary>
        /// Gets the locations where jobs can be based
        /// </summary>
        public Dictionary<int,string> Locations { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets the job types.
        /// </summary>
        public Dictionary<int, string> JobTypes { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets the salary ranges.
        /// </summary>
        public Dictionary<int, string> SalaryRanges { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets the work patterns.
        /// </summary>
        /// <value>
        /// The work patterns.
        /// </value>
        public Dictionary<int, string> WorkPatterns { get; } = new Dictionary<int, string>();
    }
}