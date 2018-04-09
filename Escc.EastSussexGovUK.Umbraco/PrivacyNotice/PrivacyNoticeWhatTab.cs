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
        [UmbracoProperty("Editor notes: Examples", "EditorNotesExamples", EditorNotesExamplesDataType.PropertyEditor, EditorNotesExamplesDataType.DataTypeName, sortOrder: 1)]
        public string EditorNotesExamples { get; set; }

        [UmbracoProperty("What does this privacy notice cover?", "WhatIsCovered", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, mandatory: true,
    description: "'This privacy notice covers...' - Explain who this privacy notice applies to and in what context.", sortOrder: 2)]
        public string WhatIsCovered { get; set; }

        [UmbracoProperty("Editor notes: Data protection taken seriously", "EditorNotesTakenSeriously", EditorNotesTakenSeriouslyDataType.PropertyEditor, EditorNotesTakenSeriouslyDataType.DataTypeName, sortOrder: 3)]
        public string EditorNotesTakenSeriously { get; set; }

        [UmbracoProperty("What is personal data?", "EditorNotesPersonalData", EditorNotesPersonalDataType.PropertyEditor, EditorNotesPersonalDataType.DataTypeName, sortOrder: 4)]
        public string EditorNotesPersonalData { get; set; }

        [UmbracoProperty("What personal data is being used?", "WhatIsUsed", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, mandatory: true,
            description: "'In order to provide the service, we will collect...' - list the data being processed, eg name, address, details of services being received, ethnicity etc.", sortOrder: 5)]
        public string WhatIsUsed { get; set; }

        [UmbracoProperty("Editor notes: Staff trained", "EditorNotesStaffTrained", EditorNotesStaffTrainedDataType.PropertyEditor, EditorNotesStaffTrainedDataType.DataTypeName, sortOrder: 6)]
        public string EditorNotesStaffTrained { get; set; }
    }
}