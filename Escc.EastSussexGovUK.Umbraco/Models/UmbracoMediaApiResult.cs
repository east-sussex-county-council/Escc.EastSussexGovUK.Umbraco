using System;
using Escc.EastSussexGovUK.Umbraco.ApiControllers;
using Newtonsoft.Json;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// A result returned by the <see cref="MediaController"/> API
    /// </summary>
    public class UmbracoMediaApiResult
    {
        /// <summary>
        /// Gets or sets the size in kilobytes.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        [JsonProperty("size")]
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        [JsonProperty("type")]
        public string Extension { get; set; }


        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}