using System.Collections.Generic;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.DataTypes;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location
{
    public static class OpeningHoursDataType
    {
        public static void CreateDataType()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>()
            {
               {"SecondSet",new PreValue(-1,"1",1)},
               {"dropdownTimestep",new PreValue(-1,"30",2)},
               {"enableClear", new PreValue(-1,"1",3)},
               {"enableAutofill", new PreValue(-1,"1",4)}
            };

            UmbracoDataTypeService.InsertDataType("Opening hours", "Jumoo.OpeningSoon", DataTypeDatabaseType.Ntext, preValues);
        }
    }
}