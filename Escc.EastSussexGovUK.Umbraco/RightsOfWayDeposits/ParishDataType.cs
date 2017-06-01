using Escc.EastSussexGovUK.Umbraco.DocumentTypes;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Umbraco data type for selecting an East Sussex parish
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(ParishDataType), DataTypeDatabaseType.Nvarchar)]
    internal class ParishDataType : PreValueListDataType
    {
        internal const string DataTypeName = "Parish";
        internal const string PropertyEditor = BuiltInUmbracoDataTypes.DropDown;

        /// <summary>
        /// Gets the parishes which can be selected using this data type
        /// </summary>
        internal static IEnumerable<string> Parishes = new string[] {
            "Parish A",
            "Parish B",
            "Parish C"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ParishDataType"/> class.
        /// </summary>
        public ParishDataType() : base(Parishes)
        {
        }
    }
}