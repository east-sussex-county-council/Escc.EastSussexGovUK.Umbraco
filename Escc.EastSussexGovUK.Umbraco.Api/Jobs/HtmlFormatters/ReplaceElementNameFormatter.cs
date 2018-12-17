using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
{
    /// <summary>
    /// Changed one type of named element into another wherever it occurs
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class ReplaceElementNameFormatter : IHtmlAgilityPackHtmlFormatter
    {
        private readonly string _fromElement;
        private readonly string _toElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceElementNameFormatter"/> class.
        /// </summary>
        /// <param name="fromElement">The incorrect element name.</param>
        /// <param name="toElement">The desired element name.</param>
        public ReplaceElementNameFormatter(string fromElement, string toElement)
        {
            _fromElement = fromElement;
            _toElement = toElement;
        }

        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void FormatHtml(HtmlDocument htmlDocument)
        {
            var nodes = htmlDocument.DocumentNode.SelectNodes($"//{_fromElement}");
            foreach (var node in nodes)
            {
                node.Name = _toElement;
            }
        }
    }
}