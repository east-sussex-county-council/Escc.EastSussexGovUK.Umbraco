using Escc.Elibrary;
using Escc.Umbraco.PropertyTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.UrlTransformers
{
    /// <summary>
    /// Recognise and convert an Elibrary proxy link to a link for the current implementation of the elibrary
    /// </summary>
    /// <seealso cref="Escc.Umbraco.PropertyTypes.IUrlTransformer" />
    public class ElibraryUrlTransformer : IUrlTransformer
    {
        /// <summary>
        /// Updates the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public Uri TransformUrl(Uri url)
        {
            return (url == null) ? url : new ElibraryProxyLinkConverter(new SpydusUrlBuilder()).RewriteElibraryUrl(url);
        }
    }
}