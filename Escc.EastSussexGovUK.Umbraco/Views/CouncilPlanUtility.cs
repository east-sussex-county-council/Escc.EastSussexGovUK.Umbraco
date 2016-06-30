using System.Web;
using EsccWebTeam.Data.Web;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public static class CouncilPlanUtility
    {
        public static string PriorityClass()
        {
            // Default return value
            var rtn = "council-plan";
            var propertyVal = string.Empty;

            try
            {
                if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return rtn;
                propertyVal = PriorityClass(UmbracoContext.Current.PageId.Value);
            }
            finally
            {
                if (!string.IsNullOrEmpty(propertyVal))
                {
                    rtn = propertyVal;
                }
            }

            return rtn;
        }

        public static string PriorityClass(int pageId)
        {
            // Default return value
            var rtn = "council-plan";
            var propertyVal = string.Empty;

            try
            {
                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var currentPage = umbracoHelper.TypedContent(pageId);

                propertyVal = currentPage.GetPropertyValue<string>("PhDefPriorityClass_Content");
            }
            finally
            {
                if (!string.IsNullOrEmpty(propertyVal))
                {
                    rtn = propertyVal;
                }
            }

            return rtn;
        }

        public static void SetContentPolicy()
        {
            var policy = new ContentSecurityPolicy(HttpContext.Current.Request.Url);
            policy.ParsePolicy(HttpContext.Current.Response.Headers["Content-Security-Policy"], true);
            policy.AppendFromConfig("CouncilPlan");
            policy.UpdateHeader(HttpContext.Current.Response);
        }
    }
}