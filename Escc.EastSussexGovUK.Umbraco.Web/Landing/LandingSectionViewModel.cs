using System.Collections.Generic;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Web.Landing
{
    /// <summary>
    /// A heading link and optional set of child links which appear in landing navigation
    /// </summary>
    public class LandingSectionViewModel
    {
        public HtmlLink Heading { get; set; }
        public IList<HtmlLink> Links { get; set; }
    }
}