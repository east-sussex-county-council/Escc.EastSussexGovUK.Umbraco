using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes
{
    /// <summary>
    /// A data-type for a date value which is set by code, not by the user
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, null, DataTypeDatabaseType.Date)]
    public class ReadOnlyDateDataType
    {
        /// <summary>
        /// The data type display name
        /// </summary>
        public const string DataTypeName = "Date (read-only)";

        /// <summary>
        /// The property editor 
        /// </summary>
        public const string PropertyEditor = BuiltInUmbracoDataTypes.NoEdit;
    }
}