using System;
using System.Collections.Generic;
using System.Web;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Banners
{
    /// <summary>
    /// A promotional banner and its display settings
    /// </summary>
    public class Banner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Banner"/> class.
        /// </summary>
        public Banner() { TargetUrls = new List<Uri>(); }

        /// <summary>
        /// Gets or sets the banner image to display.
        /// </summary>
        public Image BannerImage { get; set; }

        /// <summary>
        /// Gets or sets the URL the banner links to.
        /// </summary>
        public Uri BannerLink { get; set; }

        /// <summary>
        /// Gets or sets the page URLs to display the banner on.
        /// </summary>
        public IList<Uri> TargetUrls { get; private set; }

        /// <summary>
        /// Gets or sets whether this banner inherits those from parent pages, or replaces them.
        /// </summary>
        public bool Inherit { get; set; }

        /// <summary>
        /// Gets or sets whether to cascade this banner to child pages.
        /// </summary>
        public bool Cascade { get; set; }
    }
}