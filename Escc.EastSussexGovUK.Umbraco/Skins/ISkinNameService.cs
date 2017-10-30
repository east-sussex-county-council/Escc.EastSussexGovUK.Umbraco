namespace Escc.EastSussexGovUK.Umbraco.Skins
{
    /// <summary>
    /// Looks up the name of the skin for a page
    /// </summary>
    public interface ISkinToApplyService
    {
        /// <summary>
        /// Looks up the name of the skin for a page.
        /// </summary>
        string LookupSkinForPage(int pageId);
    }
}