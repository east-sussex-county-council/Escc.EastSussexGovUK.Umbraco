using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.UserControls
{
    /// <summary>
    /// Settings to use when displaying the section navigation usercontrol for templates migrated from Microsoft CMS
    /// </summary>
    public class SectionNavigationSettings
    {
        /// <summary>
        /// Name of the CMS placeholder to use for the links
        /// </summary>
        public string HtmlPlaceholderToBind { get; set; }

        /// <summary>
        /// Name of the CMS placeholder to use for the image
        /// </summary>
        public string ImagePlaceholderToBind { get; set; }

        /// <summary>
        /// Gets or sets the layout view in use for this request
        /// </summary>
        public EsccWebsiteView EsccWebsiteView { get; set; }
    }
}