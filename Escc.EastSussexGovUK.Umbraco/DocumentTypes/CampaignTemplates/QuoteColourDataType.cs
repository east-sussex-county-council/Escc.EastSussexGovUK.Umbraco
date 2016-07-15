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
    /// Umbraco data type for selecting which colours for quotes on a campaign content template
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(QuoteColourDataType), DataTypeDatabaseType.Nvarchar)]
    internal class QuoteColourDataType : IPreValueProvider
    {
        internal const string DataTypeName = "Campaign quote colour";
        internal const string PropertyEditor = BuiltInUmbracoDataTypes.ColorPicker;

        /// <summary>
        /// Gets the colours which can be selected using this data type
        /// </summary>
        internal static IEnumerable<string> Colours = new string[] {
            "50A030",
            "3E6C96",
            "E07D10",
            "E53A45"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="QuoteColourDataType"/> class.
        /// </summary>
        public QuoteColourDataType()
        {
            PreValues = CreatePreValues();
        }

        private static IDictionary<string, PreValue> CreatePreValues()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>();

            // Use a SHA1 key for each prevalue because:
            // * we want prevalues to be accessible programatically
            // * it can be generated from the prevalue, in a way that remains consistent if more prevalues are added later
            // * SHA1 is short enough to fit the Umbraco database schema
            // * it doesn't need to be secure, just unique
            using (HashAlgorithm algorithm = SHA1.Create())
            {
                foreach (var prevalue in Colours)
                {
                    preValues.Add(HashString(algorithm, prevalue), new PreValue(-1, prevalue));
                }
            }

            return preValues;
        }

        private static string HashString(HashAlgorithm algorithm, string wasteType)
        {
            var key = new StringBuilder();
            var data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(wasteType));
            foreach (byte hashByte in data)
            {
                key.Append(hashByte.ToString("x2", CultureInfo.InvariantCulture));
            }
            return key.ToString();
        }

        /// <summary>
        /// Gets the colours which can be selected
        /// </summary>
        /// <value>
        /// The pre values.
        /// </value>
        public IDictionary<string, PreValue> PreValues { get; private set; }
    }
}
