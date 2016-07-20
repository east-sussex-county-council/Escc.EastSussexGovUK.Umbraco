using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location
{
    /// <summary>
    /// Umbraco data type for selecting which council is responsible for a service location
    /// </summary>
    internal static class ResponsibleAuthorityDataType
    {
        internal const string DataTypeName = "Responsible authority";
        internal const string PropertyEditor = BuiltInUmbracoDataTypes.RadioButtonList;

        internal static void CreateDataType()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>()
            {
               {"0",new PreValue(-1,"East Sussex County Council")},
               {"1",new PreValue(-1,"Eastbourne Borough Council")},          
               {"2",new PreValue(-1,"Hastings Borough Council")},
               {"3",new PreValue(-1,"Lewes District Council")},
               {"4",new PreValue(-1,"Rother District Council")},
               {"5",new PreValue(-1,"Wealden District Council")}
            };

            UmbracoDataTypeService.InsertDataType(DataTypeName, PropertyEditor, DataTypeDatabaseType.Integer, preValues);
        }
    }
}