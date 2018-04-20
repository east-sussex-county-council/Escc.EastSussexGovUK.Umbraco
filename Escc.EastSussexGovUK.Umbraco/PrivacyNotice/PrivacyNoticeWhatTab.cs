using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// Umbraco properties for creating one section of a privacy notice
    /// </summary>
    public class PrivacyNoticeWhatTab : TabBase
    {
        [UmbracoProperty("What is personal data?", "EditorNotesPersonalData", EditorNotesPersonalDataType.PropertyEditor, EditorNotesPersonalDataType.DataTypeName, sortOrder: 1)]
        public string EditorNotesPersonalData { get; set; }

        [UmbracoProperty("What personal data is being used?", "WhatIsUsed", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, mandatory: true,
            description: "'In order to provide the service, we will collect...' - list the data being processed, eg name, address, details of services being received, ethnicity etc.", sortOrder: 2)]
        public string WhatIsUsed { get; set; }
    }
}