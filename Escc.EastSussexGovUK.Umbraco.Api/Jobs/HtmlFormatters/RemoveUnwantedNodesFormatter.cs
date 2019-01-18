using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
{
    /// <summary>
    /// Removes unwanted nodes from the DOM, optionally leaving their content
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class RemoveUnwantedNodesFormatter : IHtmlAgilityPackHtmlFormatter
    {
        private readonly IEnumerable<string> _nodesToRemove;
        private readonly bool _removeChildNodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUnwantedNodesFormatter" /> class.
        /// </summary>
        /// <param name="nodesToRemove">XPath selector for the nodes to remove, eg my-element or comment().</param>
        /// <param name="removeChildNodes"><c>true</c> if child nodes including text nodes should be removed; <c>false</c> otherwise</param>
        public RemoveUnwantedNodesFormatter(IEnumerable<string> nodesToRemove, bool removeChildNodes=false)
        {
            _nodesToRemove = nodesToRemove ?? throw new ArgumentNullException(nameof(nodesToRemove));
            _removeChildNodes = removeChildNodes;
        }

        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void FormatHtml(HtmlDocument htmlDocument)
        {
            if (htmlDocument == null) return;

            foreach (var nodeToRemove in _nodesToRemove)
            {
                var matchedNodes = htmlDocument.DocumentNode.SelectNodes("//" + nodeToRemove);
                if (matchedNodes != null)
                {
                    foreach (var node in matchedNodes)
                    {
                        if (!_removeChildNodes)
                        {
                            foreach (var child in node.ChildNodes)
                            {
                                node.ParentNode.InsertAfter(child, node);
                            }
                        }
                        node.ParentNode.RemoveChild(node);
                    }
                }
            }
        }
    }
}