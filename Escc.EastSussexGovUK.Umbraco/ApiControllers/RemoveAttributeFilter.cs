using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.ApiControllers
{
    /// <summary>
    /// Removes an HTML attribute wherever it appears
    /// </summary>
    public class RemoveAttributeFilter : IHtmlAgilityPackFilter
    {
        private readonly string _attributeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveAttributeFilter"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute to remove.</param>
        /// <exception cref="System.ArgumentNullException">attributeName</exception>
        public RemoveAttributeFilter(string attributeName)
        {
            if (String.IsNullOrEmpty(attributeName)) throw new ArgumentNullException(nameof(attributeName));
            _attributeName = attributeName;
        }

        /// <summary>
        /// Filters the specified HTML document.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void Filter(HtmlDocument htmlDocument)
        {
            if (htmlDocument == null) throw new ArgumentNullException(nameof(htmlDocument));

            var elements = htmlDocument.DocumentNode.SelectNodes($"//*[@{_attributeName}]");
            foreach (var element in elements)
            {
                element.Attributes.Remove(_attributeName);
            }

        }
    }
}