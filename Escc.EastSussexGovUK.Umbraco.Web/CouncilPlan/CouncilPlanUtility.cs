using System.Collections.Specialized;
using System.Web;
using Escc.Web;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.CouncilPlan
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

        public static void SetContentPolicy(NameValueCollection responseHeaders)
        {
            var policy = new ContentSecurityPolicyHeaders(responseHeaders);
            policy.AppendPolicy(new ContentSecurityPolicyFromConfig().Policies["CouncilPlan"]);
            policy.UpdateHeaders();
        }
    }
}