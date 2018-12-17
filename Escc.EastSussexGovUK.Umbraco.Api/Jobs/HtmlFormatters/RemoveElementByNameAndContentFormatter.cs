using System;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
{
    /// <summary>
    /// Removes an element from HTML if both the element name and the text content match supplied values
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class RemoveElementByNameAndContentFormatter : IHtmlAgilityPackHtmlFormatter
    {
        private readonly string _elementName;
        private readonly string _elementContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveElementByNameAndContentFormatter"/> class.
        /// </summary>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="elementContent">Content of the element.</param>
        /// <exception cref="System.ArgumentNullException">
        /// elementName
        /// or
        /// elementContent
        /// </exception>
        public RemoveElementByNameAndContentFormatter(string elementName, string elementContent)
        {
            if (String.IsNullOrEmpty(elementName)) throw new ArgumentNullException(nameof(elementName));
            if (String.IsNullOrEmpty(elementContent)) throw new ArgumentNullException(nameof(elementContent));
            _elementName = elementName;
            _elementContent = elementContent;
        }

        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void FormatHtml(HtmlDocument htmlDocument)
        {
            var nodes = htmlDocument.DocumentNode.SelectNodes($"//{_elementName}");
            foreach (HtmlNode node in nodes)
            {
                if (node.InnerHtml == _elementContent) { node.Remove(); }
            }
        }
    }
}