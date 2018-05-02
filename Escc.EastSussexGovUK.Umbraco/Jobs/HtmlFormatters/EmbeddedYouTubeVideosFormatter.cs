using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.HtmlFormatters
{
    /// <summary>
    /// Adds an 'embed' class to YouTube video links which can be picked up by JavaScript embed code
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class EmbeddedYouTubeVideosFormatter : IHtmlAgilityPackHtmlFormatter
    {
        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void FormatHtml(HtmlDocument htmlDocument)
        {
            if (htmlDocument == null) return;

            var matchedElements = htmlDocument.DocumentNode.SelectNodes("//a");
            if (matchedElements != null)
            {
                foreach (var element in matchedElements)
                {
                    if (Regex.IsMatch(element.GetAttributeValue("href",string.Empty), @"https?:\/\/(youtu.be\/|www.youtube.com\/watch\?v=)([A-Za-z0-9_-]+)", RegexOptions.IgnoreCase))
                    {
                        var className = (element.GetAttributeValue("class", string.Empty) + " embed").TrimStart();
                        element.SetAttributeValue("class", className);
                    }
                }
            }
        }
    }
}