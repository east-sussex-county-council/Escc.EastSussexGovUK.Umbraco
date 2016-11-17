using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.ApiControllers
{
    /// <summary>
    /// Removes an HTML element based on the value of its id attribute
    /// </summary>
    public class RemoveElementByIdFilter : IHtmlAgilityPackFilter
    {
        private readonly string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveElementByIdFilter"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.ArgumentNullException">id</exception>
        public RemoveElementByIdFilter(string id)
        {
            if (String.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            _id = id;
        }

        /// <summary>
        /// Filters the specified HTML document.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        /// <exception cref="System.ArgumentNullException">htmlDocument</exception>
        public void Filter(HtmlDocument htmlDocument)
        {
            if (htmlDocument == null) throw new ArgumentNullException(nameof(htmlDocument));

            var elementToRemove = (HtmlNodeNavigator)htmlDocument.CreateNavigator().SelectSingleNode($"//*[@id='{_id}']");
            if (elementToRemove != null)
            {
                elementToRemove.CurrentNode.ParentNode.RemoveChild(elementToRemove.CurrentNode);
            }
        }
    }
}