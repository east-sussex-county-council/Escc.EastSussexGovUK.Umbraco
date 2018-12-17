using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Web.Guide
{
    /// <summary>
    /// A link to be presented as part of guide navigation
    /// </summary>
    public class GuideNavigationLink : HtmlLink
    {
        /// <summary>
        /// Gets or sets whether this instance is the current page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is current page; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrentPage { get; set; }
    }
}