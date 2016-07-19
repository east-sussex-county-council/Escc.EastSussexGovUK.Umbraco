using System;
using System.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Views.Layouts
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