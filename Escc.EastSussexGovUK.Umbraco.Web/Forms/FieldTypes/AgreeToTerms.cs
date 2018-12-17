using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Web.Forms.FieldTypes
{
    /// <summary>
    /// An Umbraco Forms field type that asks people to agree to terms set out in a separate document
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class AgreeToTerms : FieldType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgreeToTerms"/> class.
        /// </summary>
        public AgreeToTerms()
        {
            Id = new Guid("e1a1ce71-a1c1-4cd2-912f-25e87d0e5275");
            Name = "Agree to terms";
            Description = "A checkbox with a link to terms. Make it mandatory.";
            DataType = FieldDataType.Bit;
            FieldTypeViewName = "FieldType.AgreeToTerms.cshtml";
            Icon = "icon-legal";
            HideLabel = true;
            SupportsPrevalues = false;
            SupportsRegex = false;
            SortOrder = 100;
        }

        [Setting("Checkbox label", view = "richdisplayedlinks")]
        public string CheckboxLabel { get; set; }
    }
}