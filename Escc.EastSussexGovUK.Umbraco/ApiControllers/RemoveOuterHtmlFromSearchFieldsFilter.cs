using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.ApiControllers
{
    /// <summary>
    /// Removes the outer HTML from the search fields page, leaving just the HTML fragment containing the fields
    /// </summary>
    public class RemoveOuterHtmlFromSearchFieldsFilter : IHtmlAgilityPackFilter
    {
        /// <summary>
        /// Filters the specified HTML document.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void Filter(HtmlDocument htmlDocument)
        {
            var elementContainingTheForm = (HtmlNodeNavigator)htmlDocument.CreateNavigator().SelectSingleNode("/html/body/div");
            if (elementContainingTheForm == null) return;
            
            // HTML Agility Pack can isolate the form element but can't correctly select its children, so fall back on regex to remove the outer element
            var formHtml = elementContainingTheForm.CurrentNode.InnerHtml;
            formHtml = Regex.Replace(formHtml, "</?form[^>]*>", String.Empty);

            htmlDocument.LoadHtml(formHtml.Trim());
        }
    }
}