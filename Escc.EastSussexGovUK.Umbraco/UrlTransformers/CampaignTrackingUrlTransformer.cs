using Escc.Umbraco.PropertyTypes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.UrlTransformers
{
    /// <summary>
    /// Adds or updates Google Analytics campaign tracking for a URL
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IUrlTransformer" />
    public class CampaignTrackingUrlTransformer : IUrlTransformer
    {
        private readonly string _source;
        private readonly string _medium;
        private readonly string _campaign;
        private readonly string _content;
        private readonly string _matchDomainsRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignTrackingUrlTransformer" /> class.
        /// </summary>
        /// <param name="source">The source parameter for Google Analytics campaign tracking.</param>
        /// <param name="medium">The medium parameter for Google Analytics campaign tracking.</param>
        /// <param name="campaign">The campaign parameter for Google Analytics campaign tracking.</param>
        /// <param name="content">The content parameter for Google Analytics campaign tracking.</param>
        /// <param name="matchDomainsRegex">Only transform links to these domains. Domains are matched using regular expression syntax.</param>
        /// <exception cref="System.ArgumentNullException">source
        /// or
        /// medium
        /// or
        /// campaign</exception>
        public CampaignTrackingUrlTransformer(string source, string medium, string campaign, string content="", string matchDomainsRegex="")
        {
            if (String.IsNullOrEmpty(source)) throw new ArgumentNullException(nameof(source));
            if (String.IsNullOrEmpty(medium)) throw new ArgumentNullException(nameof(medium));
            if (String.IsNullOrEmpty(campaign)) throw new ArgumentNullException(nameof(campaign));

            _source = source;
            _medium = medium;
            _campaign = campaign;
            _content = content;
            _matchDomainsRegex = matchDomainsRegex;
        }

        /// <summary>
        /// Transforms the URL.
        /// </summary>
        /// <param name="urlToTransform">The URL to transform.</param>
        /// <returns></returns>
        public Uri TransformUrl(Uri urlToTransform)
        {
            if (urlToTransform.IsAbsoluteUri)
            {
                if (String.IsNullOrEmpty(_matchDomainsRegex) || Regex.IsMatch(urlToTransform.Host, _matchDomainsRegex, RegexOptions.IgnoreCase))
                {
                    var query = HttpUtility.ParseQueryString(urlToTransform.Query);

                    // Add default utm_source if there's currently no campaign tracking
                    if (String.IsNullOrEmpty(query["utm_source"]))
                    {
                        query["utm_source"] = _source;
                    }

                    // Always update the medium
                    query["utm_medium"] = _medium;

                    // Add the default utm_content only if there's no campaign, because utm_content is optional in an existing campaign link
                    if (String.IsNullOrEmpty(query["utm_content"]) && String.IsNullOrEmpty(query["utm_campaign"]) && !String.IsNullOrEmpty(_content))
                    {
                        query["utm_content"] = _content;
                    }

                    // Add default utm_campaign if there's currently no campaign tracking
                    if (String.IsNullOrEmpty(query["utm_campaign"]))
                    {
                        query["utm_campaign"] = _campaign;
                    }
                    urlToTransform = new Uri(urlToTransform.Scheme + "://" + urlToTransform.Authority + urlToTransform.AbsolutePath + "?" + query);
                }
            }
            return urlToTransform;
        }
    }
}