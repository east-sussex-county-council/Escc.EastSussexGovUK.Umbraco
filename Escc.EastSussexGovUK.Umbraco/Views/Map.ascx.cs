using System;
using System.Web.Mvc;
using Escc.Umbraco.MicrosoftCmsMigration;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public partial class Map : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.latest.LatestHtml = Model.Latest.ToString();

            var phXhtml = CmsUtilities.Placeholders["phDefImageMapXhtml"];

            // reference imagemap if supplied - expects <map name="map"></map>
            // (NOTE: <map id="map"></map> doesn't work in Moz, so use name attribute
            string imageMapXhtml = phXhtml.XmlAsString;
            if (!String.IsNullOrEmpty(imageMapXhtml) && imageMapXhtml.Length != this.phImageMap.DefaultXml.Length)
            {
                this.phMap.AssociatedMapId = "map";
            }
        }
    }
}