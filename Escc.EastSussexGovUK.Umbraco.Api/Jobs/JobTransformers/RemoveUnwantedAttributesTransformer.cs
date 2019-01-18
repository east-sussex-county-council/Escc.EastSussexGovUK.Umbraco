using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers
{
    /// <summary>
    /// Removes the specified attributes wherever they occur in the supplied HTML document
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers.IJobTransformer" />
    public class RemoveUnwantedAttributesTransformer : IJobTransformer
    {
        private readonly IEnumerable<string> _attributesToRemove;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUnwantedAttributesTransformer"/> class.
        /// </summary>
        /// <param name="attributesToRemove">The attributes to remove.</param>
        /// <exception cref="System.ArgumentNullException">attributesToRemove</exception>
        public RemoveUnwantedAttributesTransformer(IEnumerable<string> attributesToRemove)
        {
            if (attributesToRemove == null) throw new ArgumentNullException(nameof(attributesToRemove));
            _attributesToRemove = attributesToRemove;
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

            foreach (var attribute in _attributesToRemove)
            {
                var nodes = htmlDocument.DocumentNode.SelectNodes($"//*[@{attribute}]");
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        node.Attributes.Remove(attribute);
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