using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration;
using Umbraco.Core.Models;
using Escc.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views
{
    public partial class CouncilPlanBudgetPage : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CouncilPlanUtility.SetContentPolicy(Response.Headers);

            Uri svgImageUrl = null;

            IPublishedContent phDefSvg = (IPublishedContent)CmsUtilities.Placeholders["phDefSvg"].Value;
            if (phDefSvg != null)
            {
                svgImageUrl = new Uri(phDefSvg.Url, UriKind.Relative);
            }

            var html = new StringBuilder();

            html.Append("<div class=\"intrinsic-container\">");
            html.Append("<iframe src=\"" + svgImageUrl + "\" allowfullscreen></iframe>");
            html.Append("</div>");

            objectTag.Text = html.ToString();
        }

        private static string FormatImageTag(string imageUrl, string altText, bool imgMap)
        {
            var imgMapStr = imgMap ? " usemap=\"#map\" class=\"image-map\"" : "";

            return string.Format(CultureInfo.InvariantCulture, "<img src=\"{0}\" alt=\"{1}\"{2} />", imageUrl, altText, imgMapStr);
        }
    }
}