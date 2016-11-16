using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// A standard Lumesse TalentLink component, which may be configurable in the TalentLink back-office
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class JobsComponentViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the URL of the script to embed the component in the page.
        /// </summary>
        /// <value>
        /// The script URL.
        /// </value>
        public Uri ScriptUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL to link to the component on a separate page.
        /// </summary>
        /// <value>
        /// The link URL.
        /// </value>
        public Uri LinkUrl { get; set; }
    }
}