using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// Umbraco properties for creating one section of a privacy notice
    /// </summary>
    public class PrivacyNoticeSharingTab : TabBase
    {
        [UmbracoProperty("Why might personal data be shared on a 'need to know' basis?", "NeedToKnow", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, 
description: "Insert reasons why personal data is shared, eg obligations to comply with legislation, court orders etc.", sortOrder: 1)]
        public string NeedToKnow { get; set; }

        [UmbracoProperty("Editor notes: Sharing", "EditorNotesSharing", EditorNotesSharingDataType.PropertyEditor, EditorNotesSharingDataType.DataTypeName, sortOrder: 2)]
        public string EditorNotesSharing { get; set; }

        [UmbracoProperty("Why might personal data be shared with third party companies?", "ThirdParty", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName,
description: "Insert reason why personal data is shared with companies, eg 'to enable us to provide one or more services'.", sortOrder: 3)]
        public string ThirdParty { get; set; }

        [UmbracoProperty("Editor notes: Contractual obligation", "EditorNotesContract", EditorNotesContractDataType.PropertyEditor, EditorNotesContractDataType.DataTypeName, sortOrder: 4)]
        public string EditorNotesContract { get; set; }
    }
}