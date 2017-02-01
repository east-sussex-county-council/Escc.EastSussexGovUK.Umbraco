using System;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.HomePage;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Schools.TermDates.Website;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Views.TermDates
{
    public partial class QuickAnswerMvcProxy : ViewUserControl<HomePageViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Model.TermDatesDataUrl != null)
            {
                var cache = UmbracoContext.Current.InPreviewMode ? null : HttpContext.Current.Cache;
                var control = (QuickAnswer) LoadControl("~/Views/TermDates/QuickAnswer.ascx");
                control.TermDatesDataProvider = new UrlProvider(new Uri(Request.Url, Model.TermDatesDataUrl), cache);
                this.Controls.Add(control);
            }
        }
    }
}