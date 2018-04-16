using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// Umbraco properties for creating one section of a privacy notice
    /// </summary>
    public class PrivacyNoticeOverviewTab : TabBase
    {
        [UmbracoProperty("Editor notes: Examples", "EditorNotesExamples", EditorNotesExamplesDataType.PropertyEditor, EditorNotesExamplesDataType.DataTypeName, sortOrder: 1)]
        public string EditorNotesExamples { get; set; }

        [UmbracoProperty("What does this privacy notice cover?", "WhatIsCovered", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, mandatory: true,
    description: "'This privacy notice covers...' - Explain who this privacy notice applies to and in what context.", sortOrder: 2)]
        public string WhatIsCovered { get; set; }

        [UmbracoProperty("Editor notes: Data protection taken seriously", "EditorNotesTakenSeriously", EditorNotesTakenSeriouslyDataType.PropertyEditor, EditorNotesTakenSeriouslyDataType.DataTypeName, sortOrder: 3)]
        public string EditorNotesTakenSeriously { get; set; }
    }
}