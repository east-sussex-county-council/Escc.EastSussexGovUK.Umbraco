using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters
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

            var matchedLinks = htmlDocument.DocumentNode.SelectNodes("//a");
            if (matchedLinks != null)
            {
                foreach (var link in matchedLinks)
                {
                    if (Regex.IsMatch(link.GetAttributeValue("href",string.Empty), @"https?:\/\/(youtu.be\/|www.youtube.com\/watch\?v=)([A-Za-z0-9_-]+)", RegexOptions.IgnoreCase))
                    {
                        var className = (link.GetAttributeValue("class", string.Empty) + " embed").TrimStart();
                        link.SetAttributeValue("class", className);
                    }
                }
            }


            var matchedWrappers = htmlDocument.DocumentNode.SelectNodes("//div[@class='video-embed-wrapper']");
            if (matchedWrappers != null)
            {
                foreach (var wrapper in matchedWrappers)
                {
                    var matchedIframes = wrapper.SelectNodes("./iframe");
                    if (matchedIframes != null)
                    {
                        foreach (var iframe in matchedIframes)
                        {
                            var match = Regex.Match(iframe.GetAttributeValue("src", string.Empty), @"(https?:)?\/\/(youtu.be\/|www.youtube.com\/)(watch\?v=|embed/)(?<VideoId>[A-Za-z0-9_-]+)(\?[^" + "\"]+)?", RegexOptions.IgnoreCase);
                            if (match.Success)
                            {
                                var replaceWith = htmlDocument.CreateElement("p");
                                var replaceWithLink = htmlDocument.CreateElement("a");
                                replaceWithLink.SetAttributeValue("class", "embed");
                                replaceWithLink.SetAttributeValue("href", "https://www.youtube.com/watch?v=" + match.Groups["VideoId"].Value);
                                replaceWithLink.InnerHtml = "Watch the video on YouTube";
                                replaceWith.ChildNodes.Add(replaceWithLink);

                                wrapper.ParentNode.InsertBefore(replaceWith, wrapper);
                                wrapper.Remove();
                            }
                        }
                    }
                }
            }
        }
    }
}