﻿using System;
using System.Web.Mvc;

namespace Escc.EastSussexGovUK.Umbraco.Views.Layouts
{
    public partial class BreadcrumbTrail : ViewUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.breadcrumb.BreadcrumbProvider = new UmbracoBreadcrumbProvider();
        }
    }
}