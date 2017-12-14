using Escc.EastSussexGovUK.Umbraco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Forms
{
    /// <summary>
    /// Model for a form built using Umbraco Forms
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class FormModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the form unique identifier.
        /// </summary>
        /// <value>
        /// The form unique identifier.
        /// </value>
        public Guid FormGuid { get; set; }

        /// <summary>
        /// Gets or sets the HTML text that appears before the form fields.
        /// </summary>
        public IHtmlString LeadingText { get; set; }
    }
}