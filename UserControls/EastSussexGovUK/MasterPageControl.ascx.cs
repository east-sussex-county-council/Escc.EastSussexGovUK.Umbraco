using System;

namespace Escc.EastSussexGovUK.Umbraco.UserControls.EastSussexGovUK
{
    public partial class MasterPageControl : System.Web.UI.UserControl
    {
        public string Control { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.masterPageControl.Control = Control;
            this.masterPageControl.BreadcrumbProvider = new UmbracoBreadcrumbProvider();
        }
    }
}