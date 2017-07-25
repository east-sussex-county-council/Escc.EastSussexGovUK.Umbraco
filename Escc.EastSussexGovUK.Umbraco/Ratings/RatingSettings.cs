using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Ratings
{
    /// <summary>
    /// Settings for a page rating service
    /// </summary>
    public class RatingSettings
    {
        /// <summary>
        /// Gets or sets the URL to rate a page as poor
        /// </summary>
        public Uri PoorUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL to rate a page as adequate
        /// </summary>
        public Uri AdequateUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL to rate a page as good
        /// </summary>
        public Uri  GoodUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL to rate a page as excellent
        /// </summary>
        public Uri ExcellentUrl { get; set; }
    }
}