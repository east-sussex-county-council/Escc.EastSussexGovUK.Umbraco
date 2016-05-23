using System;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Schools.TermDates.Website;
using EsccWebTeam.Data.Web;

namespace Escc.EastSussexGovUK.Umbraco.Views.TermDates
{
    public partial class QuickAnswerMvcProxy : ViewUserControl<HomePageViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Model.TermDatesDataUrl != null)
            {
                var control = (QuickAnswer) LoadControl("~/Views/TermDates/QuickAnswer.ascx");
                control.TermDatesDataProvider = new UrlProvider(Iri.MakeAbsolute(Model.TermDatesDataUrl), HttpContext.Current.Cache);
                this.Controls.Add(control);
            }
        }
    }
}