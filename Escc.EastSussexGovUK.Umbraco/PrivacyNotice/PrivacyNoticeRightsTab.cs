using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// Umbraco properties for creating one section of a privacy notice
    /// </summary>
    public class PrivacyNoticeRightsTab : TabBase
    {
        [UmbracoProperty("Editor notes: Rights", "EditorNotesRights", EditorNotesRightsDataType.PropertyEditor, EditorNotesRightsDataType.DataTypeName, sortOrder: 1)]
        public string EditorNotesRights { get; set; }
    }
}