using Escc.EastSussexGovUK.Umbraco.DocumentTypes;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Skins
{
    /// <summary>
    /// Umbraco data type for selecting a skin
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(SkinDataType), DataTypeDatabaseType.Nvarchar)]
    internal class SkinDataType : PreValueListDataType
    {
        internal const string DataTypeName = "Skin";
        internal const string PropertyEditor = BuiltInUmbracoDataTypes.DropDown;

        /// <summary>
        /// Gets the skins which can be selected using this data type
        /// </summary>
        internal static IEnumerable<string> Skins = new string[] {
            "Foster with trust"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinDataType"/> class.
        /// </summary>
        public SkinDataType() : base(Skins)
        {
        }
    }
}