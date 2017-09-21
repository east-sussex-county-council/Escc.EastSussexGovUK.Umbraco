using Escc.EastSussexGovUK.Umbraco.DocumentTypes;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Location
{
    /// <summary>
    /// Umbraco data type for selecting which waste types are accepted at a recycling site
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(WasteTypesDataType), DataTypeDatabaseType.Nvarchar)]
    internal class WasteTypesDataType : PreValueListDataType
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
            "Paint – solvent-based",
            "Paint – water-based emulsion",
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
        public WasteTypesDataType() : base(WasteTypes)
        {
        }
    }
}