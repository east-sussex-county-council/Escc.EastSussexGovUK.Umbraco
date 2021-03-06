﻿using System;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Web.HomePage;
using Escc.Net.Configuration;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views.TermDates
{
    public partial class QuickAnswerMvcProxy : ViewUserControl<HomePageViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Model.TermDatesDataUrl != null)
            {
                var cache = UmbracoContext.Current.InPreviewMode ? null : HttpContext.Current.Cache;
                var control = (QuickAnswer) LoadControl("~/Views/TermDates/QuickAnswer.ascx");
                control.TermDatesDataProvider = new UrlProvider(new Uri(Request.Url, Model.TermDatesDataUrl), cache, new ConfigurationProxyProvider());
                this.Controls.Add(control);
            }
        }
    }
}