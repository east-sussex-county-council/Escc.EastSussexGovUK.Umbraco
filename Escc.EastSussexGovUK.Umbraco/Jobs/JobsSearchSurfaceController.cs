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

            var queryString = new NameValueCollection();
            if (!String.IsNullOrEmpty(query.Keywords)) queryString.Add("keywords", query.Keywords);
            if (!String.IsNullOrEmpty(query.JobReference)) queryString.Add("ref", query.JobReference);

            foreach (var value in query.JobTypes) queryString.Add("type", value);
            foreach (var value in query.Locations) queryString.Add("location", value);
            foreach (var value in query.Organisations) queryString.Add("org", value);
            foreach (var value in query.SalaryRanges) queryString.Add("salary", value);
            foreach (var value in query.WorkPatterns) queryString.Add("hours", value);

            return new RedirectToUmbracoPageResult(resultsPage, queryString);
        }
    }
}