using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Web.Forms.FieldTypes
{
    /// <summary>
    /// An Umbraco Forms field type that asks for gender in the GOV.UK-recommended way
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    /// <remarks>
    /// https://www.gov.uk/service-manual/design/gender-or-sex
    /// </remarks>
    public class Gender : Escc.Umbraco.Forms.FieldTypes.Gender
    {
        [Setting("Privacy notice", description = "What happens to the data?")]
        public string PrivacyNotice { get; set; }
    }
}