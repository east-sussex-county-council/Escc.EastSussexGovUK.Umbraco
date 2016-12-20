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
        public ActionResult Search(JobSearchFilter filter)
        {
            var resultsPage = CurrentPage.GetPropertyValue<IPublishedContent>("SearchResultsPage_Content");
            if (resultsPage == null) return null;

            var query = new NameValueCollection();
            if (!String.IsNullOrEmpty(filter.Keywords)) query.Add("keywords", filter.Keywords);
            if (!String.IsNullOrEmpty(filter.JobReference)) query.Add("jobnum", filter.JobReference);

            foreach (var value in filter.JobTypes) query.Add("LOV40", value);
            foreach (var value in filter.Locations) query.Add("LOV39", value);
            foreach (var value in filter.Organisations) query.Add("LOV52", value);
            foreach (var value in filter.SalaryRanges) query.Add("LOV46", value);
            foreach (var value in filter.WorkPatterns) query.Add("LOV50", value);

            return new RedirectToUmbracoPageResult(resultsPage, query);
        }
    }
}