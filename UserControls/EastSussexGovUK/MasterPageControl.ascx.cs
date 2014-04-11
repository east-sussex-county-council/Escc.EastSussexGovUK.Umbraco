using System;
using System.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.UserControls.EastSussexGovUK
{
    public partial class MasterPageControl : ViewUserControl<MasterPageControlData>
    {
        public string Control { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.masterPageControl.Control = Model.Control;
            this.masterPageControl.BreadcrumbProvider = new UmbracoBreadcrumbProvider();
        }
    }
}