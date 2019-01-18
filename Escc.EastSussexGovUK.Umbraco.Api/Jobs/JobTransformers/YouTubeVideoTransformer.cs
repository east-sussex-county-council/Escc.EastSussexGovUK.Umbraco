using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers
{
    /// <summary>
    /// Adds an 'embed' class to YouTube video links which can be picked up by JavaScript embed code
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IHtmlAgilityPackHtmlFormatter" />
    public class YouTubeVideoTransformer : IJobTransformer
    {
        /// <summary>
        /// Formats an HTML fragment.
        /// </summary>
        /// <param name="html">The HTML .</param>
        private IHtmlString TransformHtml(IHtmlString html)
        {
            if (html == null) return html;

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.ToHtmlString());

            if (htmlDocument.DocumentNode == null) return html;

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

            return new HtmlString(htmlDocument.DocumentNode.OuterHtml);
        }

        public void TransformJob(Job job)
        {
            job.AdvertHtml = TransformHtml(job.AdvertHtml);
            job.AdditionalInformationHtml = TransformHtml(job.AdditionalInformationHtml);
            job.EqualOpportunitiesHtml = TransformHtml(job.EqualOpportunitiesHtml);
        }
    }
}