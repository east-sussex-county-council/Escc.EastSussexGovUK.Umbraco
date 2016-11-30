using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Services
{
    /// <summary>
    /// Modify a URL in a way defined by the implementation
    /// </summary>
    public interface IUrlTransformer
    {
        /// <summary>
        /// Transforms the URL.
        /// </summary>
        /// <param name="urlToTransform">The URL to transform.</param>
        /// <returns></returns>
        Uri TransformUrl(Uri urlToTransform);
    }
}