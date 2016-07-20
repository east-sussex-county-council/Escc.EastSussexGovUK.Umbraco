using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.SocialMedia
{
    /// <summary>
    /// An Umbraco data type used to select whether to show a widget on a web page
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(ShowWidgetDataType), DataTypeDatabaseType.Integer)]
    public class ShowWidgetDataType : IPreValueProvider
    {
        public const string DataTypeName = "Show widget";
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
                    {"Show", new PreValue(-1, "Show")},
                    {"Hide", new PreValue(-1, "Hide")},
                    {"Inherit", new PreValue(-1, "Inherit")},
                };
            }
        }
    }
}