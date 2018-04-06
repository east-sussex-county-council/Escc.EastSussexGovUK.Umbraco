using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// Umbraco properties for creating one section of a privacy notice
    /// </summary>
    public class PrivacyNoticeWhyTab : TabBase
    {
        [UmbracoProperty("How will personal data be used?", "Why", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, mandatory: true,
    description: "List the purposes for which the data will be used, eg to enable delivery of a statutory duty, to safeguard children, to improve outcomes for service users.", sortOrder: 1)]
        public string Why { get; set; }

        [UmbracoProperty("Is personal data being processed outside the European Economic Area (EEA)?", "OutsideEEA", BuiltInUmbracoDataTypes.Boolean, sortOrder: 2)]
        public string OutsideEEA { get; set; }

        [UmbracoProperty("Is automated decision-making being used?", "AutomatedDecisionMaking", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, 
    description: "If automated decision making is used, state here and refer to guidance for data subject rights.", sortOrder: 3)]
        public string AutomatedDecisionMaking { get; set; }
    }
}