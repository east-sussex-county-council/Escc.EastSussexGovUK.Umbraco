using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;

namespace Escc.EastSussexGovUK.Umbraco.Web.Forms.FieldTypes
{
    /// <summary>
    /// An autocomplete field for Umbraco Forms for selecting a state-funded school in East Sussex
    /// </summary>
    /// <seealso cref="Umbraco.Forms.Core.FieldType" />
    public class School : FieldType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="School"/> class.
        /// </summary>
        public School()
        {
            Id = new Guid("1224a224-745e-474e-8d46-37c5fd5d6f4d");
            Name = "School";
            Description = "A state-funded school in East Sussex.";
            DataType = FieldDataType.String;
            FieldTypeViewName = "FieldType.School.cshtml";
            Icon = "icon-presentation";
            HideLabel = false;
            SupportsPrevalues = false;
            SupportsRegex = false;
            SortOrder = 100;
        }
    }
}