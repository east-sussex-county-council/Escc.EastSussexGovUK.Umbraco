using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.ApiControllers
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
