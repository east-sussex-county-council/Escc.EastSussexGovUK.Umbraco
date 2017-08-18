using System.Xml.XPath;

namespace Escc.EastSussexGovUK.Umbraco.Views.TermDates
{
    /// <summary>
    /// A source of term dates XML data
    /// </summary>
    public interface ITermDatesDataProvider
    {
        /// <summary>
        /// Gets the term dates data as a <see cref="XPathDocument"/>.
        /// </summary>
        /// <returns></returns>
        IXPathNavigable GetXPathData();
    }
}