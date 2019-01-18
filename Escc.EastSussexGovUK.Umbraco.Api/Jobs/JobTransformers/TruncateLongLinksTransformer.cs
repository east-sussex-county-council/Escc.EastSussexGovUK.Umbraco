using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.Html;
using HtmlAgilityPack;
using System;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers
{
    /// <summary>
    /// Look for link text that looks like a URL, and abbreviate it to prevent non-wrapping text overflowing its container
    /// </summary>
    public class TruncateLongLinksTransformer : IJobTransformer
    {
        private readonly IHtmlLinkFormatter _linkFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruncateLongLinksTransformer"/> class.
        /// </summary>
        /// <param name="linkFormatter">The link formatter.</param>
        /// <exception cref="System.ArgumentNullException">linkFormatter</exception>
        public TruncateLongLinksTransformer(IHtmlLinkFormatter linkFormatter)
        {
            this._linkFormatter = linkFormatter ?? throw new System.ArgumentNullException(nameof(linkFormatter));
        }

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

            var links = htmlDocument.DocumentNode.SelectNodes("//a");
            if (links == null) return html;

            foreach (var link in links)
            {
                if (link.InnerHtml.StartsWith("http:") || link.InnerHtml.StartsWith("https:"))
                {
                    link.InnerHtml = _linkFormatter.AbbreviateUrl(new Uri(link.InnerHtml, UriKind.RelativeOrAbsolute));
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