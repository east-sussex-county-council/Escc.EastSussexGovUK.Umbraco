using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage
{
    /// <summary>
    /// An Umbraco data type used by the Standard Landing Page template
    /// </summary>
    public static class LandingPageColumnsDataType
    {
        public const string DataTypeName = "Landing page columns";

        public static void CreateDataType()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>()
            {
               {"0",new PreValue(-1,"Auto",1)},
               {"1",new PreValue(-1,"2 columns",2)},
               {"2", new PreValue(-1,"3 columns",3)}
            };

            UmbracoDataTypeService.InsertDataType(DataTypeName, BuiltInUmbracoDataTypes.DropDown, DataTypeDatabaseType.Nvarchar, preValues);
        }
    }
}