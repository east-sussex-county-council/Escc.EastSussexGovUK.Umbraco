using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Format an HTML string
    /// </summary>
    public interface IHtmlStringFormatter
    {
        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="html">The HTML document.</param>
        string FormatHtml(string html);
    }
}
