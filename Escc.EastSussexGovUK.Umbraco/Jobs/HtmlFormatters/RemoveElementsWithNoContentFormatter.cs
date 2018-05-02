using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.HtmlFormatters
{
    /// <summary>
    /// Removes elements which contain either no children, or a text node with only white space
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class RemoveElementsWithNoContentFormatter : IHtmlAgilityPackHtmlFormatter
    {
        private readonly IEnumerable<string> _elementsToRemove;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveElementsWithNoContentFormatter" /> class.
        /// </summary>
        /// <param name="elementsToRemove">The elements to remove.</param>
        public RemoveElementsWithNoContentFormatter(IEnumerable<string> elementsToRemove)
        {
            _elementsToRemove = elementsToRemove ?? throw new ArgumentNullException(nameof(elementsToRemove));
        }

        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void FormatHtml(HtmlDocument htmlDocument)
        {
            if (htmlDocument == null) return;

            foreach (var elementToRemove in _elementsToRemove)
            {
                var matchedElements = htmlDocument.DocumentNode.SelectNodes("//" + elementToRemove);
                if (matchedElements != null)
                {
                    foreach (var element in matchedElements)
                    {
                        if (Regex.IsMatch(element.InnerHtml, @"^\s*$"))
                        {
                            element.ParentNode.RemoveChild(element);
                        }
                    }
                }
            }
        }
    }
}