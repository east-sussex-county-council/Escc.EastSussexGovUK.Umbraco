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
               {"GoogleMap", new PreValue(-1,"Image on right (formerly Google map)",2)},
               {"Alternative",new PreValue(-1,"Image on left",3)},
               {"FeaturedImage", new PreValue(-1,"Large image",4)},
               {"ImageRow", new PreValue(-1,"Row of images",5)},
               {"ImageRowBorderless", new PreValue(-1,"Row of images without borders",6)},
               {"ChildrenLibrary", new PreValue(-1,"Children's library links",7)},
               {"ChildrenBack", new PreValue(-1,"Back to children's section",8)},
               {"SchoolClosures", new PreValue(-1,"School closures list",10)},
               {"TermDates", new PreValue(-1,"School term dates",11)},
               {"OpenElectionData", new PreValue(-1,"Open election data landing page",13)},
               {"Gritting", new PreValue(-1,"Gritting",14)},
               {"FloodAlerts", new PreValue(-1,"Flood alerts (mobile)",16)},
               {"RecyclingSiteSearch", new PreValue(-1,"Find a recycling site",17)}
            };

            UmbracoDataTypeService.InsertDataType(DataTypeName, BuiltInUmbracoDataTypes.DropDown, DataTypeDatabaseType.Nvarchar, preValues);
        }
    }
}