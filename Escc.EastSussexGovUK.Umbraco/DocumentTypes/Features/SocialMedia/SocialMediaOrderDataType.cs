using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Core.Models;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.SocialMedia
{
    public static class SocialMediaOrderDataType
    {
        public static void CreateSocialMediaOrderDataType()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>()
            {
               {"Twitter, Facebook",new PreValue(-1,"Twitter, Facebook")},
               {"Facebook, Twitter",new PreValue(-1,"Facebook, Twitter")}
            };

            UmbracoDataTypeService.InsertDataType("Social media order", "Umbraco.RadioButtonList", DataTypeDatabaseType.Integer, preValues);
        }
    }
}