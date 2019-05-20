using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Mvc;
using Escc.Web;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.Web.TextSize
{
    /// <summary>
    /// Manage the text size on the website
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class TextSizeController : Controller
    {
        // GET: Sets a cookie based on a querystring parameter, which has the effect of changing the sitewide text size
        public async Task<ActionResult> Change()
        {
            var model = new TextSizeViewModel
            {
                Metadata =
                {
                    Title = "Text size changed",
                    IsInSearch = false,
                    DateIssued = new DateTimeOffset(new DateTime(2010, 4, 19))
                }
            };

            if (Request.QueryString["textsize"] != null && Request.QueryString["textsize"].Length == 1)
            {
                HttpCookie textSize = new HttpCookie("textsize", Request.QueryString["textsize"]);
                textSize.Expires = DateTime.Now.AddMonths(1);
                if (Request.Url.Host.IndexOf("eastsussex.gov.uk", StringComparison.Ordinal) > -1) textSize.Domain = ".eastsussex.gov.uk";
                Response.Cookies.Add(textSize);
            }

            // Add a cache-busting parameter so that the user isn't returned to an HTTP-cached version of the page which has the old text size
            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath != Request.Url.AbsolutePath)
            {
                var referrerQuery = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
                referrerQuery.Remove("nocache");
                referrerQuery.Add("nocache", Guid.NewGuid().ToString());
                var redirectTo = new Uri(Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority + Request.UrlReferrer.AbsolutePath + "?" + referrerQuery);
                new HttpStatus().SeeOther(redirectTo);
            }

            var templateRequest = new EastSussexGovUKTemplateRequest(Request);
            try
            {
                // Do this to load the template controls.
                model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }

            return View(model);
        }
    }
}