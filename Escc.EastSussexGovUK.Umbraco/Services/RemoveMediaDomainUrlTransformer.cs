using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Removes the domain from Umbraco media URLs, leaving a domain-relative link like /media/1234/image.jpg
    /// </summary>
    /// <seealso cref="IMediaUrlTransformer" />
    /// <remarks>This is required to handle absolute URLs pasted in by web authors which should be relative</remarks>
    public class RemoveMediaDomainUrlTransformer : IMediaUrlTransformer
    {
        /// <summary>
        /// Transforms the media URL.
        /// </summary>
        /// <param name="mediaUrl">The media URL.</param>
        /// <returns></returns>
        public Uri TransformMediaUrl(Uri mediaUrl)
        {
            if (mediaUrl == null) throw new ArgumentNullException(nameof(mediaUrl));

            if (mediaUrl.IsAbsoluteUri && mediaUrl.PathAndQuery.ToUpperInvariant().StartsWith("/MEDIA/"))
            {
                return new Uri(mediaUrl.PathAndQuery, UriKind.Relative);
            }

            return mediaUrl;
        }

        /// <summary>
        /// Parses HTML and transforms any media urls found.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>
        /// Updated HTML
        /// </returns>
        public string ParseAndTransformMediaUrlsInHtml(string html)
        {
            html = ReplaceImageUrlsInHtml(html);

            html = ReplaceLinksInHtml(html);

            return html;
        }

        private string ReplaceLinksInHtml(string html)
        {
            var matchesLinks = Regex.Matches(html, @"<a(?<attr1>.*?)href=""(?<url>[^""]*/media/.*?)""(?<attr2>.*?)>(?<content>.*?)</a>");
            foreach (Match match in matchesLinks)
            {
                var url = new Uri(match.Groups["url"].Value, UriKind.RelativeOrAbsolute);
                if (url.IsAbsoluteUri)
                {
                    var content = match.Groups["content"].Value;
                    var outputLink = String.Format(@"<a{0}href=""{1}""{2}>{3}</a>", match.Groups["attr1"].Value, url.PathAndQuery, match.Groups["attr2"].Value, content);
                    html = html.Replace(match.Groups[0].Value, outputLink);
                }
            }
            return html;
        }

        private string ReplaceImageUrlsInHtml(string html)
        {
            var matchesImages = Regex.Matches(html, @"<img(?<attr1>.*?)src=""(?<url>[^""]*/media/.*?)""(?<attr2>.*?)/>");
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
