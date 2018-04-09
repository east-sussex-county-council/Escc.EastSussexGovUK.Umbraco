using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// Umbraco properties for creating one section of a privacy notice
    /// </summary>
    public class PrivacyNoticeContactTab : TabBase
    {
        [UmbracoProperty("Editor notes: Contact", "EditorNotesContact", EditorNotesContactDataType.PropertyEditor, EditorNotesContactDataType.DataTypeName, sortOrder: 1)]
        public string EditorNotesContact { get; set; }

        [UmbracoProperty("Contact details for your service", "Contact", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, mandatory:true,
description: "Your service's contact details to find out about how personal data is used, eg Name of department, email address, phone number.", sortOrder: 2)]
        public string Contact { get; set; }

        [UmbracoProperty("Editor notes: Complain", "EditorNotesComplain", EditorNotesComplainDataType.PropertyEditor, EditorNotesComplainDataType.DataTypeName, sortOrder: 3)]
        public string EditorNotesComplain { get; set; }
    }
}