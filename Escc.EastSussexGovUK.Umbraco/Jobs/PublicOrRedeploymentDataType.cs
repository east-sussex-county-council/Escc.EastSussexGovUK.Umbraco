using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// An Umbraco data type used to select which jobs index to query
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(PublicOrRedeploymentDataType), DataTypeDatabaseType.Integer)]
    public class PublicOrRedeploymentDataType : IPreValueProvider
    {
        public const string DataTypeName = "Public jobs or redeployment jobs";
        public const string PropertyEditor = BuiltInUmbracoDataTypes.RadioButtonList;

        /// <summary>
        /// Gets the pre values.
        /// </summary>
        /// <value>
        /// The pre values.
        /// </value>
        public IDictionary<string, PreValue> PreValues
        {
            get
            {
                return new Dictionary<string, PreValue>()
                {
                    {"Public jobs", new PreValue(-1, "Public jobs")},
                    {"Redeployment jobs", new PreValue(-1, "Redeployment jobs")}
                };
            }
        }
    }
}