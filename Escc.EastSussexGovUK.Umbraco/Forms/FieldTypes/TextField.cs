using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Providers.FieldTypes;
using Umbraco.Forms.Core.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Forms.FieldTypes
{
    /// <summary>
    /// A text field for Umbraco Forms with an additional property for displaying a pop-up privacy notice
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class TextField : Textfield
    {
        [Setting("Privacy notice", description = "What happens to the data?")]
        public string PrivacyNotice { get; set; }
    }
}