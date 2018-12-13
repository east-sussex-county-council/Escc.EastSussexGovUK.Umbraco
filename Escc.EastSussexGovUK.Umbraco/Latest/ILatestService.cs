namespace Escc.EastSussexGovUK.Umbraco.Latest
{
    /// <summary>
    /// Service to get 'latest' content 
    /// </summary>
    public interface ILatestService
    {
        /// <summary>
        /// Build a composite 'latest' from the current page and its ancestors
        /// </summary>
        /// <returns></returns>
        LatestSettings ReadLatestSettings();
    }
}