using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Guide
{
    /// <summary>
    /// Umbraco document type definition for configuring the navigation p in a guide
    /// </summary>
    public class GuideNavigationTab : TabBase
    {
        [UmbracoProperty("Section navigation", "SectionNavigation", SectionNavigationDataType.PropertyEditor, SectionNavigationDataType.DataTypeName, sortOrder: 1)]
        public string SectionNavigation { get; set; }

    }
}