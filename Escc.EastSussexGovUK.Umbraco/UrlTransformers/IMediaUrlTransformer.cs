using System;

namespace Escc.EastSussexGovUK.Umbraco.UrlTransformers
{
    /// <summary>
    /// Update Umbraco media URLs
    /// </summary>
    public interface IMediaUrlTransformer
    {
        /// <summary>
        /// Parses HTML and transforms any media urls found.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>Updated HTML</returns>
        string ParseAndTransformMediaUrlsInHtml(string html);

        /// <summary>
        /// Transforms the media URL.
        /// </summary>
        /// <param name="mediaUrl">The media URL.</param>
        /// <returns></returns>
        Uri TransformUrl(Uri mediaUrl);
    }
}