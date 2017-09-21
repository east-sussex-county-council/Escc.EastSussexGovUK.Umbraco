using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Removes elements which contain either no children, or a text node with only white space
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class RemoveElementsWithNoContentFormatter : IHtmlAgilityPackHtmlFormatter
    {
        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void FormatHtml(HtmlDocument htmlDocument)
        {
            if (htmlDocument == null) return;

            var elementsToRemove = new[] { "strong", "p" };

            foreach (var elementToRemove in elementsToRemove)
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