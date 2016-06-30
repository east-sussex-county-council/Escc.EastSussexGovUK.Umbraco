using System;
using System.Text;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public partial class CouncilPlanPrioritiesPage : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CouncilPlanUtility.SetContentPolicy();

            var svg = CmsUtilities.Placeholders["phDefSvg"].Value.ToString();

            var pngPh = CmsUtilities.Placeholders["phDefFallbackImage"].Value;
            var html = new StringBuilder();
            if (!String.IsNullOrEmpty(svg))
            {
                html.Append("<object type=\"image/svg+xml\" data=\"").Append(svg).Append("\">");
            }
            if (pngPh != null)
            {
                var png = CmsUtilities.Placeholders["phDefFallbackImage"].Value.ToString().Replace(" border=\"0\"", " /");
                html.Append(png);
            }
            if (!String.IsNullOrEmpty(svg))
            {
                html.Append("</object>");
            }
            objectTag.Text = html.ToString();
        }
    }
}