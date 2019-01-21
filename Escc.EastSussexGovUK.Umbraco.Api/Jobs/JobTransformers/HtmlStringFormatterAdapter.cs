using Escc.EastSussexGovUK.Umbraco.Jobs;
using System;
using System.Collections.Generic;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers
{
    /// <summary>
    /// Apply one or more <see cref="IHtmlStringFormatter"/>s to the HTML of a job advert
    /// </summary>
    public class HtmlStringFormatterAdapter : IJobTransformer
    {
        private readonly IEnumerable<IHtmlStringFormatter> _formatters;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlStringFormatterAdapter"/> class.
        /// </summary>
        /// <param name="formatters">The HTML formatters.</param>
        /// <exception cref="System.ArgumentNullException">formatters</exception>
        public HtmlStringFormatterAdapter(IEnumerable<IHtmlStringFormatter> formatters)
        {
            this._formatters = formatters ?? throw new System.ArgumentNullException(nameof(formatters));
        }

        /// <summary>
        /// Formats an HTML fragment.
        /// </summary>
        /// <param name="html">The HTML .</param>
        private IHtmlString TransformHtml(IHtmlString html)
        {
            if (html == null) return html;

            var formattedHtml = html.ToHtmlString();

            foreach (var formatter in _formatters)
            {
                if (formatter == null) continue;
                formattedHtml = formatter.FormatHtml(formattedHtml);
            }

            return new HtmlString(formattedHtml);
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