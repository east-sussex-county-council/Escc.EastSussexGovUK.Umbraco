using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates
{
    /// <summary>
    /// An Umbraco data type used to set the prefered styling for the social media and sharing links
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(ShareStyleDataType), DataTypeDatabaseType.Nvarchar)]
    public class ShareStyleDataType : IPreValueProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShareStyleDataType"/> class.
        /// </summary>
        public ShareStyleDataType()
        {
            PreValues = new Dictionary<string, PreValue>()
            {
               {"Default",new PreValue(-1,"Default",1)},
               {"White",new PreValue(-1,"White",2)},
               {"White on a dark panel",new PreValue(-1,"White on a dark panel",3)}
            };
        }

        public const string DataTypeName = "Share style";
        public const string PropertyEditor = BuiltInUmbracoDataTypes.DropDown;

        public IDictionary<string, PreValue> PreValues { get; }
    }
}