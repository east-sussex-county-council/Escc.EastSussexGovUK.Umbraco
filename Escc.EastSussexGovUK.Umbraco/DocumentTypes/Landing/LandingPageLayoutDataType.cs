using System.Collections.Generic;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.DataTypes;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing
{
    internal static class LandingPageLayoutDataType
    {
        internal static void CreateLandingPageLayoutDataType()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>()
            {
               {"Three columns (default)",new PreValue(-1,"Three columns (default)")},
               {"Two columns",new PreValue(-1,"Two columns")},          
            };

            UmbracoDataTypeService.InsertDataType("Landing page layout", BuiltInUmbracoDataTypes.RadioButtonList, DataTypeDatabaseType.Integer, preValues);
        }
    }
}