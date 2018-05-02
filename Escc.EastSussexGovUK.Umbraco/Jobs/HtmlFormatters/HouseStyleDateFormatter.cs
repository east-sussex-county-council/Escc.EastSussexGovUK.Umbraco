using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.HtmlFormatters
{
    /// <summary>
    /// Tidies up inconsistent formatting of dates embedded in body text into a standard British date which conforms with our house style
    /// </summary>
    /// <seealso cref="IHtmlStringFormatter" />
    public class HouseStyleDateFormatter : IHtmlStringFormatter
    {
        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="html">The HTML document.</param>
        /// <returns></returns>
        public string FormatHtml(string html)
        {
                      // We're not American
            html = Regex.Replace(html, @"(January|February|March|April|May|June|July|August|September|October|November|December)\s+([0-9]{1,2})(st|nd|rd|th)?\b", "$2 $1");

            // Remove ordinals
            html = Regex.Replace(html, @"([0-9]{1,2})(<sup>)?(st|nd|rd|th)\s*(<\/sup>)?\s*(January|February|March|April|May|June|July|August|September|October|November|December)", "$1 $5");

            // Remove comma between month and year
            html = Regex.Replace(html, @"(January|February|March|April|May|June|July|August|September|October|November|December),\s+([0-9]{4})\b", "$1 $2");

            // Remove comma between day and date
            html = Regex.Replace(html, @"(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday),\s+([0-9]{1,2})\s+(January|February|March|April|May|June|July|August|September|October|November|December)", "$1 $2 $3");

            // Remove leading zeroes from dates
            html = Regex.Replace(html, @"0([0-9])\s+(January|February|March|April|May|June|July|August|September|October|November|December)(\s+[0-9]{4})?\b", "$1 $2$3");

            return html;
        }
    }
}