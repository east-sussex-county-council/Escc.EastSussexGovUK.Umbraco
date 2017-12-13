using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Forms.FieldTypes
{
    /// <summary>
    /// A text field for Umbraco Forms with an additional property for displaying a pop-up privacy notice
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class TextFieldPersonalData : FieldType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextFieldPersonalData"/> class.
        /// </summary>
        public TextFieldPersonalData()
        {
            Id = new Guid("f8b54c1f-b4b6-4120-a13d-1635aff7f6e4");
            Name = "Short answer (personal data)";
            Description = "Renders an text input field, for short answers, with an optional pop-up privacy notice";
            Icon = "icon-autofill";
            Category = "Simple";
            DataType = FieldDataType.String;
            SortOrder = 3;
            FieldTypeViewName = "FieldType.TextFieldPersonalData.cshtml";
        }

        [Setting("Privacy notice", description = "What happens to the data?")]
        public string PrivacyNotice { get; set; }

        [Setting("Default Value", description = "Enter a default value")]
        public string DefaultValue { get; set; }

        [Setting("Placeholder", description = "Enter a HTML5 placeholder value", view = "TextField")]
        public string Placeholder { get; set; }

        public override bool SupportsRegex
        {
            get
            {
                return true;
            }
        }
    }
}