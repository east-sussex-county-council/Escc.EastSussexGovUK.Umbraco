using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location
{
    /// <summary>
    /// Umbraco data type for selecting which waste types are accepted at a recycling site
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(WasteTypesDataType), DataTypeDatabaseType.Nvarchar)]
    internal class WasteTypesDataType : IPreValueProvider
    {
        internal const string DataTypeName = "Waste types";
        internal const string PropertyEditor = BuiltInUmbracoDataTypes.CheckBoxList;
        
        /// <summary>
        /// Gets the waste types which can be selected using this data type
        /// </summary>
        internal static IEnumerable<string> WasteTypes = new string[] {
            "Aluminium foil",
            "Aerosols",
            "Bonded asbestos",
            "Books",
            "Bric-a-brac",
            "Car batteries",
            "Cans/Tins",
            "Cardboard",
            "Cassettes",
            "CDs and cases",
            "Chemicals",
            "Cooking oil",
            "Electrical goods",
            "Engine oil",
            "Fluorescent tubes/Energy saving bulbs",
            "Furniture",
            "Fridges/Freezers",
            "Glass bottles/Jars",
            "Green garden waste/Christmas trees",
            "Hard plastics (for example, plastic toys and furniture)",
            "Hardcore/Rubble",
            "Household batteries",
            "Household waste",
            "Metal items",
            "Mobile phones",
            "Newspapers/Magazines/Junk mail/White Telephone Directories",
            "Paint – water-based emulsion",
            "Paint – solvent-based",
            "Plasterboard",
            "Plastic bottles",
            "Plastic carrier bags",
            "Printer cartridges",
            "Soil",
            "Spectacles",
            "Tetra paks/Food or drink cartons",
            "Textiles/Shoes",
            "Timber/Wood",
            "Trade/Business waste",
            "TVs/Computer monitors",
            "Tyres",
            "Yellow Pages"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="WasteTypesDataType"/> class.
        /// </summary>
        public WasteTypesDataType()
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
                foreach (var wasteType in WasteTypes)
                {
                    preValues.Add(HashString(algorithm, wasteType), new PreValue(-1, wasteType));
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
        /// Gets the waste types which can be selected
        /// </summary>
        /// <value>
        /// The pre values.
        /// </value>
        public IDictionary<string, PreValue> PreValues { get; private set; }
    }
}