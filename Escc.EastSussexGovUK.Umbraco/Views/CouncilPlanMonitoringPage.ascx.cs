﻿using System;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public partial class CouncilPlanMonitoringPage : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CouncilPlanUtility.SetContentPolicy();

            // Get the parent page
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var currentPage = umbracoHelper.TypedContent(Model.PageId);
            var parentPage = currentPage.Parent;

            aimsLink.HRef = parentPage.Url;
        }
    }
}