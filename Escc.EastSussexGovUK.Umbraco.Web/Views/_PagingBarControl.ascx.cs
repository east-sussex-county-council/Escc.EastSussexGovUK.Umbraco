using Escc.NavigationControls.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views
{
    /// <summary>
    /// Proxy to use the WebForms <see cref="PagingBarControl"/> in a Razor view
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ViewUserControl{Escc.NavigationControls.WebForms.PagingController}" />
    public partial class _PagingBarControl : ViewUserControl<PagingController>
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            pagingBar.PagingController = Model;
        }
    }
}