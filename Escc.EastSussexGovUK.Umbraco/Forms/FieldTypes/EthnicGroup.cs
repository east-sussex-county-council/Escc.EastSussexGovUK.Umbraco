using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Forms.FieldTypes
{
    /// <summary>
    /// An Umbraco Forms field type that displays a dropdown list of ethnic groups
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class EthnicGroup : Escc.Umbraco.Forms.FieldTypes.EthnicGroup
    {
        [Setting("Privacy notice", description = "What happens to the data?")]
        public string PrivacyNotice { get; set; }
    }
}