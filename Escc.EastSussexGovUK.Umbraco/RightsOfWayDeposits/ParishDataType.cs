using Escc.EastSussexGovUK.Umbraco.DocumentTypes;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Umbraco data type for selecting an East Sussex parish
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(ParishDataType), DataTypeDatabaseType.Nvarchar)]
    internal class ParishDataType : PreValueListDataType
    {
        internal const string DataTypeName = "Parish";
        internal const string PropertyEditor = BuiltInUmbracoDataTypes.DropDownMultiple;

        /// <summary>
        /// Gets the parishes which can be selected using this data type
        /// </summary>
        internal static IEnumerable<string> Parishes = new string[] {
            "Alciston",
            "Alfriston",
            "Arlington",
            "Ashburnham",
            "Barcombe",
            "Battle",
            "Beckley",
            "Beddingham",
            "Berwick",
            "Bexhill",
            "Bodiam",
            "Brede",
            "Brightling",
            "Burwash",
            "Buxted",
            "Camber",
            "Catsfield",
            "Chailey",
            "Chalvington with Ripe",
            "Chiddingly",
            "Crowborough",
            "Crowhurst",
            "Cuckmere Valley",
            "Dallington",
            "Danehill",
            "Ditchling",
            "East Chiltington",
            "East Dean and Friston",
            "East Guldeford",
            "East Hoathly with Halland",
            "Eastbourne",
            "Etchingham",
            "Ewhurst",
            "Fairlight",
            "Falmer",
            "Firle",
            "Fletching",
            "Forest Row",
            "Framfield",
            "Frant",
            "Glynde",
            "Guestling",
            "Hadlow Down",
            "Hailsham",
            "Hamsey",
            "Hartfield",
            "Hastings",
            "Heathfield and Waldron",
            "Hellingly",
            "Herstmonceux",
            "Hooe",
            "Horam",
            "Hurst Green",
            "Icklesham",
            "Iden",
            "Iford",
            "Isfield",
            "Kingston Near Lewes",
            "Laughton",
            "Lewes",
            "Little Horsted",
            "Long Man",
            "Maresfield",
            "Mayfield and Five Ashes",
            "Mountfield",
            "Newhaven",
            "Newick",
            "Ninfield",
            "Northiam",
            "Peacehaven",
            "Peasmarsh",
            "Penhurst",
            "Pett",
            "Pevensey",
            "Piddinghoe",
            "Playden",
            "Plumpton",
            "Polegate",
            "Ringmer",
            "Rodmell",
            "Rotherfield",
            "Rye Foreign",
            "Rye",
            "Salehurst and Robertsbridge",
            "Seaford",
            "Sedlescombe",
            "Selmeston",
            "South Heighton",
            "Southease",
            "St Ann (Without)",
            "St John (Without)",
            "Streat",
            "Tarring Neville",
            "Telscombe",
            "Ticehurst",
            "Uckfield",
            "Udimore",
            "Wadhurst",
            "Warbleton",
            "Wartling",
            "Westfield",
            "Westham",
            "Westmeston",
            "Whatlington",
            "Willingdon and Jevington",
            "Withyham",
            "Wivelsfield"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ParishDataType"/> class.
        /// </summary>
        public ParishDataType() : base(Parishes)
        {
        }
    }
}