using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
{
    /// <summary>
    /// Removes unwanted elements from the DOM, optionally leaving their content
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class RemoveUnwantedElementsFormatter : IHtmlAgilityPackHtmlFormatter
    {
        private readonly IEnumerable<string> _elementsToRemove;
        private readonly bool _removeChildNodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUnwantedElementsFormatter" /> class.
        /// </summary>
        /// <param name="elementsToRemove">The elements to remove.</param>
        /// <param name="removeChildNodes"><c>true</c> if child nodes including text nodes should be removed; <c>false</c> otherwise</param>
        public RemoveUnwantedElementsFormatter(IEnumerable<string> elementsToRemove, bool removeChildNodes=false)
        {
            _elementsToRemove = elementsToRemove ?? throw new ArgumentNullException(nameof(elementsToRemove));
            _removeChildNodes = removeChildNodes;
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
                        if (!_removeChildNodes)
                        {
                            foreach (var child in element.ChildNodes)
                            {
                                element.ParentNode.InsertAfter(child, element);
                            }
                        }
                        element.ParentNode.RemoveChild(element);
                    }
                }
            }
        }
    }
}