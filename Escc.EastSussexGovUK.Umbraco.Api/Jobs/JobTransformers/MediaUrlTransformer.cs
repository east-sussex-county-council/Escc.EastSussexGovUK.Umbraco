using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers
{
    /// <summary>
    /// Adapter for an <see cref="IMediaUrlTransformer"/> to apply it to the HTML of job adverts
    /// </summary>
    public class MediaUrlTransformer : IJobTransformer
    {
        private readonly IMediaUrlTransformer _mediaUrlTransformer;

        public MediaUrlTransformer(IMediaUrlTransformer mediaUrlTransformer)
        {
            _mediaUrlTransformer = mediaUrlTransformer ?? throw new ArgumentNullException(nameof(mediaUrlTransformer));
        }

        /// <summary>
        /// Formats an HTML fragment.
        /// </summary>
        /// <param name="html">The HTML .</param>
        private IHtmlString TransformHtml(IHtmlString html)
        {
            if (html == null) return html;

            return new HtmlString(_mediaUrlTransformer.ParseAndTransformMediaUrlsInHtml(html.ToHtmlString()));
        }

        public void TransformJob(Job job)
        {
            job.AdvertHtml = TransformHtml(job.AdvertHtml);
            job.AdditionalInformationHtml = TransformHtml(job.AdditionalInformationHtml);
            job.EqualOpportunitiesHtml = TransformHtml(job.EqualOpportunitiesHtml);
        }
    }
}