using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.RichTextHtmlFormatters
{
    /// <summary>
    /// Turns any absolute links to *.azurewebsites.net domains within a block of HTML into relative links
    /// </summary>
    /// <seealso cref="Escc.Umbraco.PropertyEditors.RichTextPropertyEditor.IRichTextHtmlFormatter" />
    public class RemoveAzureDomainHtmlFormatter : IRichTextHtmlFormatter
    {
        public string Format(string html)
        {
            html = ReplaceImageUrlsInHtml(html);

            html = ReplaceLinksInHtml(html);

            return html;
        }

        private string ReplaceLinksInHtml(string html)
        {
            var matchesLinks = Regex.Matches(html, @"<a(?<attr1>.*?)href=""(?<url>[^""]*\.azurewebsites\.net.*?)""(?<attr2>.*?)>");
            foreach (Match match in matchesLinks)
            {
                var url = new Uri(match.Groups["url"].Value, UriKind.RelativeOrAbsolute);
                if (url.IsAbsoluteUri)
                {
                    var outputLink = String.Format(@"<a{0}href=""{1}""{2}>", match.Groups["attr1"].Value, url.PathAndQuery, match.Groups["attr2"].Value);
                    html = html.Replace(match.Groups[0].Value, outputLink);
                }
            }
            return html;
        }

        private string ReplaceImageUrlsInHtml(string html)
        {
            var matchesImages = Regex.Matches(html, @"<img(?<attr1>.*?)src=""(?<url>[^""]*\.azurewebsites\.net.*?)""(?<attr2>.*?)/>");
            foreach (Match match in matchesImages)
            {
                var url = new Uri(match.Groups["url"].Value, UriKind.RelativeOrAbsolute);
                if (url.IsAbsoluteUri)
                {
                    var outputLink = string.Format(@"<img{0}src=""{1}""{2}/>", match.Groups["attr1"].Value, url.PathAndQuery, match.Groups["attr2"].Value);
                    html = html.Replace(match.Groups[0].Value, outputLink);
                }
            }
            return html;
        }
    }
}