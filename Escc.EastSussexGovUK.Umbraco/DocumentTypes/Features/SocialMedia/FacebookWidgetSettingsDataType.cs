using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.SocialMedia
{
    public static class FacebookWidgetSettingsDataType
    {
        public static void CreateFacebookWidgetSettingsDataType()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>()
            {
               {"Show faces",new PreValue(-1,"Show faces")},
               {"Show feed",new PreValue(-1,"Show feed")}           
            };

            UmbracoDataTypeService.InsertDataType("Facebook widget settings", "Umbraco.CheckBoxList", DataTypeDatabaseType.Nvarchar, preValues);
        }
    }
}