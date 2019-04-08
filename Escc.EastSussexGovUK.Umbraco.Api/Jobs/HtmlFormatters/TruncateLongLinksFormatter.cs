using Escc.Html;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
{
    /// <summary>
    /// Look for link text that looks like a URL, and abbreviate it to prevent non-wrapping text overflowing its container
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class TruncateLongLinksFormatter : IHtmlAgilityPackHtmlFormatter
    {
        private readonly IHtmlLinkFormatter _linkFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruncateLongLinksFormatter"/> class.
        /// </summary>
        /// <param name="linkFormatter">The link formatter.</param>
        /// <exception cref="System.ArgumentNullException">linkFormatter</exception>
        public TruncateLongLinksFormatter(IHtmlLinkFormatter linkFormatter)
        {
            this._linkFormatter = linkFormatter ?? throw new System.ArgumentNullException(nameof(linkFormatter));
        }

        /// <summary>
        /// Formats the HTML.
        /// </summary>
        /// <param name="htmlDocument">The HTML document.</param>
        public void FormatHtml(HtmlDocument htmlDocument)
        {
            if (htmlDocument == null) return;

            var links = htmlDocument.DocumentNode.SelectNodes("//a");
            if (links == null) return;

            var badLinks = new List<HtmlNode>();
            foreach (var link in links)
            {
                if (link.InnerHtml.StartsWith("http:") || link.InnerHtml.StartsWith("https:"))
                {
                    try
                    {
                        link.InnerHtml = _linkFormatter.AbbreviateUrl(new Uri(link.InnerHtml, UriKind.RelativeOrAbsolute));
                    }
                    catch (UriFormatException)
                    {
                        // It's a bad link - save it for delinking
                        badLinks.Add(link);
                    }
                }
            }
            foreach (var link in badLinks)
            {
                // Delink bad links
                link.ParentNode.ReplaceChild(htmlDocument.CreateTextNode(link.InnerHtml), link);
            }
        }
    }
}