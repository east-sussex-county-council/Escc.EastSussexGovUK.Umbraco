using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
{
    /// <summary>
    /// Removes unwanted elements from the DOM but leaves their content
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class RemoveUnwantedElementsFormatter : IHtmlAgilityPackHtmlFormatter
    {
        private readonly IEnumerable<string> _elementsToRemove;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUnwantedElementsFormatter" /> class.
        /// </summary>
        /// <param name="elementsToRemove">The elements to remove.</param>
        public RemoveUnwantedElementsFormatter(IEnumerable<string> elementsToRemove)
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
                        foreach (var child in element.ChildNodes)
                        {
                            element.ParentNode.InsertAfter(child, element);
                        }
                        element.ParentNode.RemoveChild(element);
                    }
                }
            }
        }
    }
}