using Escc.EastSussexGovUK.Umbraco.DocumentTypes;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Guide
{
    /// <summary>
    /// Umbraco data type for configuring section navigation on Guide templates
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(SectionNavigationDataType), DataTypeDatabaseType.Nvarchar)]
    internal class SectionNavigationDataType : PreValueListDataType
    {
        internal const string DataTypeName = "Section navigation";
        internal const string PropertyEditor = BuiltInUmbracoDataTypes.RadioButtonList;

        /// <summary>
        /// Gets the options for configuring section navigation
        /// </summary>
        internal static IEnumerable<string> Options = new string[] {
            "Bulleted list",
            "Numbered list with 'next' and 'previous' buttons"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionNavigationDataType"/> class.
        /// </summary>
        public SectionNavigationDataType() : base(Options)
        {
        }
    }
}