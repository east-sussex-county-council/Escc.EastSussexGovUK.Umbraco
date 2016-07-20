using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan
{
    public static class PriorityDataType
    {
        public const string DataTypeName = "Council Plan Priority";

        public static void CreateDataType()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>()
            {
               {"Council plan",new PreValue(-1,"council-plan",1)},
               {"Economic growth", new PreValue(-1,"economic-growth",2)},
               {"Vulnerable people",new PreValue(-1,"vulnerable-people",3)},
               {"Building resilience", new PreValue(-1,"building-resilience",4)},
               {"Best use of resources", new PreValue(-1,"best-use-of-resources",5)}
            };

            UmbracoDataTypeService.InsertDataType(DataTypeName, BuiltInUmbracoDataTypes.DropDown, DataTypeDatabaseType.Nvarchar, preValues);
        }
    }
}