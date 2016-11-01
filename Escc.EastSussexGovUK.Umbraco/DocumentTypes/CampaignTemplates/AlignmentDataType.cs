using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates
{
    /// <summary>
    /// An Umbraco data type used to set left, right or full-width alignment
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(AlignmentDataType), DataTypeDatabaseType.Nvarchar)]
    public class AlignmentDataType : IPreValueProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlignmentDataType"/> class.
        /// </summary>
        public AlignmentDataType()
        {
            PreValues = new Dictionary<string, PreValue>()
            {
               {"Full width",new PreValue(-1,"Full width",1)},
               {"Left",new PreValue(-1,"Left",2)},
               {"Right",new PreValue(-1,"Right",3)}
            }; ;
        }

        public const string DataTypeName = "Alignment";
        public const string PropertyEditor = BuiltInUmbracoDataTypes.DropDown;

        public IDictionary<string, PreValue> PreValues { get; }
    }
}