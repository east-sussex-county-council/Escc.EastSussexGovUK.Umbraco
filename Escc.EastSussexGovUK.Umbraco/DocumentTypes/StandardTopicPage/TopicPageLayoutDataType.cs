using System.Collections.Generic;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.DataTypes;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage
{
    /// <summary>
    /// An Umbraco data type used by the Standard Topic Page template
    /// </summary>
    public static class TopicPageLayoutDataType
    {
        public const string DataTypeName = "Topic page layout";

        public static void CreateDataType()
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>()
            {
               {"Normal",new PreValue(-1,"Image on right",1)},
               {"Alternative",new PreValue(-1,"Image on left",3)},
               {"FeaturedImage", new PreValue(-1,"Large image",4)},
               {"ImageRowBorderless", new PreValue(-1,"Row of images without borders",6)},
               {"SchoolClosures", new PreValue(-1,"School closures list",10)},
               {"TermDates", new PreValue(-1,"School term dates",11)},
               {"FloodAlerts", new PreValue(-1,"Flood alerts (mobile)",16)},
               {"RecyclingSiteSearch", new PreValue(-1,"Find a recycling site",17)}
            };

            UmbracoDataTypeService.InsertDataType(DataTypeName, BuiltInUmbracoDataTypes.DropDown, DataTypeDatabaseType.Nvarchar, preValues);
        }
    }
}