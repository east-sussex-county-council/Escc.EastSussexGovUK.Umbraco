using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// A filter to modify an HTML string parsed using the HTML Agility Pack
    /// </summary>
    interface IHtmlAgilityPackFilter
    {
        /// <summary>
        /// Filters the specified HTML document.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        void Filter(HtmlDocument htmlDocument);
    }
}
