using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using AST.AzureBlobStorage.Helper;
using Escc.Umbraco.MicrosoftCmsMigration;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public partial class CouncilPlanBudgetPage : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CouncilPlanUtility.SetContentPolicy();

            Uri svgImageUrl = null;

            IPublishedContent phDefSvg = (IPublishedContent) CmsUtilities.Placeholders["phDefSvg"].Value;
            if (phDefSvg != null)
            {
                svgImageUrl = ContentHelper.TransformUrl(new Uri(phDefSvg.Url, UriKind.Relative));
            }

            var phDefFallbackImage = (IPublishedContent)CmsUtilities.Placeholders["phDefFallbackImage"].Value;
            var fallbackImageUrl = ContentHelper.TransformUrl(new Uri(phDefFallbackImage.Url, UriKind.Relative));

            var imageMapXhtml = CmsUtilities.Placeholders["phDefFallbackHtml"].XmlAsString;

            var fallbackImgStr = FormatImageTag(fallbackImageUrl.ToString(), "alt text", imageMapXhtml.Length > 0);

            var html = new StringBuilder();
            html.Append("<object type=\"image/svg+xml\" data=\"");
            html.Append(svgImageUrl);
            html.Append("\">");
            html.Append(fallbackImgStr);
            html.Append("</object>");

            objectTag.Text = html.ToString();
        }

        private static string FormatImageTag(string imageUrl, string altText, bool imgMap)
        {
            var imgMapStr = imgMap ? " usemap=\"#map\" class=\"image-map\"" : "";

            return string.Format(CultureInfo.InvariantCulture, "<img src=\"{0}\" alt=\"{1}\"{2} />", imageUrl, altText, imgMapStr);
        }
    }
}