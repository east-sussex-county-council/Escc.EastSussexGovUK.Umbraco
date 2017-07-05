using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class JobsSearchSurfaceController : SurfaceController
    {
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Search(JobSearchQuery query)
        {
            var resultsPage = CurrentPage.GetPropertyValue<IPublishedContent>("SearchResultsPage_Content");
            if (resultsPage == null) return null;

            var queryString = new JobSearchQueryConverter().ToCollection(query);

            return new RedirectToUmbracoPageResult(resultsPage, queryString);
        }
    }
}