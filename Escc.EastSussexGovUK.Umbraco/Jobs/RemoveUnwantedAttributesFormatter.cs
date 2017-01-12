using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Removes the specified attributes wherever they occur in the supplied HTML document
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class RemoveUnwantedAttributesFormatter : IHtmlAgilityPackHtmlFormatter
    {
        private readonly IEnumerable<string> _attributesToRemove;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUnwantedAttributesFormatter"/> class.
        /// </summary>
        /// <param name="attributesToRemove">The attributes to remove.</param>
        /// <exception cref="System.ArgumentNullException">attributesToRemove</exception>
        public RemoveUnwantedAttributesFormatter(IEnumerable<string> attributesToRemove)
        {
            if (attributesToRemove == null) throw new ArgumentNullException(nameof(attributesToRemove));
            _attributesToRemove = attributesToRemove;
        }

        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void FormatHtml(HtmlDocument htmlDocument)
        {
            foreach (var attribute in _attributesToRemove)
            {
                var nodes = htmlDocument.DocumentNode.SelectNodes($"//*[@{attribute}]");
                foreach (var node in nodes)
                {
                    node.Attributes.Remove(attribute);
                }
            }
        }
    }
}