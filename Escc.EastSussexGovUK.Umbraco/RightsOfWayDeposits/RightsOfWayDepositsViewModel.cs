using Escc.EastSussexGovUK.Umbraco.Models;
using System.Collections.Generic;
using Escc.NavigationControls.WebForms;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// View model for the listings page for rights of way Section 31 deposits
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class RightsOfWayDepositsViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the total deposits recorded in Umbraco
        /// </summary>
        /// <value>
        /// The total deposits.
        /// </value>
        public int TotalDeposits { get; set; }

        /// <summary>
        /// Gets the configuration for paging the results
        /// </summary>
        /// <value>
        /// The paging.
        /// </value>
        public PagingController Paging { get; internal set; }

        /// <summary>
        /// Gets or sets data on the current page of deposits
        /// </summary>
        public IList<RightsOfWayDepositViewModel> Deposits = new List<RightsOfWayDepositViewModel>();

        /// <summary>
        /// Gets or sets the sort order currently applied to deposits
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public RightsOfWayDepositsSortOrder SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the leading text.
        /// </summary>
        /// <value>
        /// The leading text.
        /// </value>
        public IHtmlString LeadingText { get; set; }
    }
}