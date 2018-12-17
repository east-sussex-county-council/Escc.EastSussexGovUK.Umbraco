using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Providers.FieldTypes;

namespace Escc.EastSussexGovUK.Umbraco.Web.Forms.FieldTypes
{
    /// <summary>
    /// A textarea field for Umbraco Forms with an additional property for displaying a pop-up privacy notice
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class TextArea : Textarea
    {
        [Setting("Privacy notice", description = "What happens to the data?")]
        public string PrivacyNotice { get; set; }
    }
}