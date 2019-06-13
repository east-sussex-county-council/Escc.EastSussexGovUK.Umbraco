using Escc.EastSussexGovUK.Umbraco.Web.Models;
using System.Collections.Generic;
using Escc.NavigationControls.WebForms;
using System.Web;
using System;

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayModifications
{
    /// <summary>
    /// View model for the listings page for rights of way definitive map modification order applications
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class RightsOfWayModificationsViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the total modification order applications recorded in Umbraco
        /// </summary>
        public int TotalModificationOrderApplications { get; set; }

        /// <summary>
        /// Gets the configuration for paging the results
        /// </summary>
        /// <value>
        /// The paging.
        /// </value>
        public PagingController Paging { get; internal set; }

        /// <summary>
        /// Gets or sets data on the current page of modification order applications
        /// </summary>
        public IList<RightsOfWayModificationViewModel> ModificationOrderApplications { get; } = new List<RightsOfWayModificationViewModel>();

        /// <summary>
        /// Gets or sets the sort order currently applied to modification order applications
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public RightsOfWayModificationsSortOrder SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the leading text.
        /// </summary>
        /// <value>
        /// The leading text.
        /// </value>
        public IHtmlString LeadingText { get; set; }

        /// <summary>
        /// Gets or sets the RSS URL.
        /// </summary>
        /// <value>
        /// The RSS URL.
        /// </value>
        public Uri RssUrl { get; set; }

        /// <summary>
        /// Gets or sets the CSV URL.
        /// </summary>
        /// <value>
        /// The CSV URL.
        /// </value>
        public Uri CsvUrl { get; set; }
    }
}