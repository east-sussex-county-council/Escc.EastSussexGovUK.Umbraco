using System;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Latest
{
    /// <summary>
    /// A 'latest' item and the rules for publishing it
    /// </summary>
    public class LatestSettings
    {
        /// <summary>
        /// The HTML to display
        /// </summary>
        public IHtmlString LatestHtml { get; set; }

        /// <summary>
        /// Publish the item at the start of this date
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// Publish the item until the end of this date
        /// </summary>
        public DateTime? UnpublishDate { get; set; }

        /// <summary>
        /// Inherit latest items from parent pages
        /// </summary>
        public bool Inherit { get; set; }

        /// <summary>
        /// Cascade this latest item to child pages
        /// </summary>
        public bool Cascade { get; set; }
    }
}