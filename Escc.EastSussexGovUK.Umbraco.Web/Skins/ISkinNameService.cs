using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.Web.Skins
{
    /// <summary>
    /// Looks up the name of the skin for a page
    /// </summary>
    public interface ISkinToApplyService
    {
        /// <summary>
        /// Looks up the name of the skin for a page.
        /// </summary>
        string LookupSkinForPage(IPublishedContent content);
    }
}