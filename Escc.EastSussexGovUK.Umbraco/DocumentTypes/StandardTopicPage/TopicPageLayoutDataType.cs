using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage
{
    /// <summary>
    /// An Umbraco data type used by the Standard Topic Page template
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(TopicPageLayoutDataType), DataTypeDatabaseType.Nvarchar)]
    public class TopicPageLayoutDataType : IPreValueProvider
    {
        public TopicPageLayoutDataType()
        {
            PreValues = new Dictionary<string, PreValue>()
            {
               {"Normal",new PreValue(-1,"Image on right",1)},
               {"Alternative",new PreValue(-1,"Image on left",3)},
               {"FeaturedImage", new PreValue(-1,"Large image",4)},
               {"ImageRowBorderless", new PreValue(-1,"Row of images without borders",6)},
               {"SchoolClosures", new PreValue(-1,"School closures list",10)},
               {"TermDates", new PreValue(-1,"School term dates",11)},
               {"RecyclingSiteSearch", new PreValue(-1,"Find a recycling site",17)}
            };
        }

        public const string DataTypeName = "Topic page layout";
        public const string PropertyEditor = BuiltInUmbracoDataTypes.DropDown;

        public IDictionary<string, PreValue> PreValues { get; }
    }
}