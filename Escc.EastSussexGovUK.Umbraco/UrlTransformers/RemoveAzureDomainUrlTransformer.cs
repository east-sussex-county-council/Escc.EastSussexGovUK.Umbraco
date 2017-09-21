using Escc.Umbraco.PropertyTypes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.UrlTransformers
{
    /// <summary>
    /// Removes the domain from *.azurewebsites.net URLs, leaving a domain-relative link like /abc/123.html
    /// </summary>
    /// <seealso cref="IUrlTransformer" />
    /// <remarks>This is required to handle absolute URLs to our back office site, pasted in by web authors, which should be relative</remarks>
    public class RemoveAzureDomainUrlTransformer : IUrlTransformer
    {
        /// <summary>
        /// Transforms the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public Uri TransformUrl(Uri url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            if (url.IsAbsoluteUri && url.Host.ToUpperInvariant().EndsWith(".AZUREWEBSITES.NET"))
            {
                return new Uri(url.PathAndQuery, UriKind.Relative);
            }

            return url;
        }
    }
}
