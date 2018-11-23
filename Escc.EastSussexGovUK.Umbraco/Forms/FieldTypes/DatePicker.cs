using System;
using System.Collections.Generic;
using Escc.Dates;
using System.Web;
using Umbraco.Forms.Core;
using fieldTypes = Umbraco.Forms.Core.Providers.FieldTypes;
using Umbraco.Forms.Core.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Forms.FieldTypes
{
    /// <summary>
    /// A date picker field for Umbraco Forms with an additional property for displaying a pop-up privacy notice
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class DatePicker : Escc.Umbraco.Forms.FieldTypes.DatePicker
    {
        [Setting("Privacy notice", description = "What happens to the data?")]
        public string PrivacyNotice { get; set; }
        
    }
}