using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Web.Forms.FieldTypes
{
    /// <summary>
    /// An Umbraco Forms field type that validates a URL
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class Url : Escc.Umbraco.Forms.FieldTypes.Url
    {
        [Setting("Privacy notice", description = "What happens to the data?")]
        public string PrivacyNotice { get; set; }
    }
}