using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features
{
    public static class CacheDataType
    {
        public static void CreateCacheDataType()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>()
            {
               {"auto",new PreValue(-1,"auto")},
               {"5 minutes",new PreValue(-1,"5 minutes")},
               {"10 minutes",new PreValue(-1,"10 minutes")},
               {"30 minutes",new PreValue(-1,"30 minutes")},
               {"1 hour",new PreValue(-1,"1 hour")}
            };

            UmbracoDataTypeService.InsertDataType("Cache", BuiltInUmbracoDataTypes.DropDown, DataTypeDatabaseType.Nvarchar, preValues);
        }
    }
}