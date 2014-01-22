using System;

namespace Escc.EastSussexGovUK.Umbraco.UserControls.EastSussexGovUK
{
    public partial class BreadcrumbTrailMobile : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.breadcrumb.BreadcrumbProvider = new UmbracoBreadcrumbProvider();
        }
    }
}