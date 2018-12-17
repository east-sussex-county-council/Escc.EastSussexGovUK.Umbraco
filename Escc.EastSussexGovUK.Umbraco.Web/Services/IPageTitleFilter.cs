namespace Escc.EastSussexGovUK.Umbraco.Web.Services
{
    /// <summary>
    /// A filter which may alter the page title provided by Umbraco
    /// </summary>
    public interface IPageTitleFilter
    {
        /// <summary>
        /// Applies the filter to the specified page title and returns the modified result
        /// </summary>
        /// <param name="pageTitle">The page title.</param>
        /// <returns></returns>
        string Apply(string pageTitle);
    }
}