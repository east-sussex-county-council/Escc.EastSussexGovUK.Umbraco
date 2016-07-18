using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location
{
    /// <summary>
    /// Content tab when editing a recycling site document type in Umbraco
    /// </summary>
    public class RecyclingSiteContentTab : LocationContentTab
    {
        [UmbracoProperty("Waste types recycled", "wasteTypes", WasteTypesDataType.PropertyEditor, WasteTypesDataType.DataTypeName, sortOrder: 30)]
        public string RecycledWasteTypes { get; set; }

        [UmbracoProperty("Waste types accepted but not recycled", "acceptedWasteTypes", WasteTypesDataType.PropertyEditor, WasteTypesDataType.DataTypeName, sortOrder: 31)]
        public string AcceptedWasteTypes { get; set; }

        [UmbracoProperty("Responsible authority", "responsibleAuthority", ResponsibleAuthorityDataType.PropertyEditor, ResponsibleAuthorityDataType.DataTypeName, sortOrder: 32)]
        public string ResponsibleAuthority { get; set; }
    }
}