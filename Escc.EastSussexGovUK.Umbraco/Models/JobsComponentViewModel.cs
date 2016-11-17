using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// A standard Lumesse TalentLink component, which may be configurable in the TalentLink back-office
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobsComponentViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the logo for the jobs service
        /// </summary>
        public Image JobsLogo { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header
        /// </summary>
        public Image HeaderBackgroundImage { get; set; }
        
        /// <summary>
        /// Gets or sets the URL of the script to embed the component in the page.
        /// </summary>
        public TalentLinkUrl ScriptUrl { get; set; }
    }
}