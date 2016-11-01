using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates
{
    /// <summary>
    /// Umbraco data type for selecting a colour from an unrestricted palette
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(ColourPickerDataType), DataTypeDatabaseType.Nvarchar)]
    internal class ColourPickerDataType : IPreValueProvider
    {
        internal const string DataTypeName = "Colour";
        internal const string PropertyEditor = "Spectrum.Color.Picker";
        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>();
    }
}
