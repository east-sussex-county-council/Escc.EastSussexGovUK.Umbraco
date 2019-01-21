using Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.Html;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers
{
    /// <summary>
    /// Apply one or more <see cref="IHtmlAgilityPackHtmlFormatter"/>s to the HTML of a job advert
    /// </summary>
    public class HtmlAgilityPackFormatterAdapter : IJobTransformer
    {
        private readonly IEnumerable<IHtmlAgilityPackHtmlFormatter> _formatters;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlAgilityPackFormatterAdapter"/> class.
        /// </summary>
        /// <param name="formatters">The HTML formatters.</param>
        /// <exception cref="System.ArgumentNullException">formatters</exception>
        public HtmlAgilityPackFormatterAdapter(IEnumerable<IHtmlAgilityPackHtmlFormatter> formatters)
        {
            _formatters = formatters ?? throw new System.ArgumentNullException(nameof(formatters));
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

            foreach (var formatter in _formatters)
            {
                if (formatter == null) continue;
                formatter.FormatHtml(htmlDocument);
            }

            return new HtmlString(htmlDocument.DocumentNode.OuterHtml);
        }

        /// <summary>
        /// Applies the formatters the the HTML properties of the <see cref="Job"/>
        /// </summary>
        /// <param name="job"></param>
        public void TransformJob(Job job)
        {
            job.AdvertHtml = TransformHtml(job.AdvertHtml);
            job.AdditionalInformationHtml = TransformHtml(job.AdditionalInformationHtml);
            job.EqualOpportunitiesHtml = TransformHtml(job.EqualOpportunitiesHtml);
        }
    }
}