using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Format HTML using the HTML Agility Pack library
    /// </summary>
    interface IHtmlAgilityPackHtmlFormatter
    {
        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        void FormatHtml(HtmlDocument htmlDocument);
    }
}
