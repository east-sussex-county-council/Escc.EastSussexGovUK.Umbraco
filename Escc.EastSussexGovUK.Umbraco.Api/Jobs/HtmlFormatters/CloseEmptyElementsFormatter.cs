using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
{
    public class CloseEmptyElementsFormatter : IHtmlStringFormatter
    {
        public string FormatHtml(string html)
        {
            var emptyElements = new[] {"br", "img"};
            foreach (var element in emptyElements)
            {
                html = Regex.Replace(html, $"<({element})( [^>]*)?([^/]?)>", $"<$1$2$3 />");
            }
            return html;
        }
    }
}