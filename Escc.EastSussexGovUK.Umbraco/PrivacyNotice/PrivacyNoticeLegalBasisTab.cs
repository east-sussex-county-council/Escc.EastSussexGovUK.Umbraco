using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// Umbraco properties for creating one section of a privacy notice
    /// </summary>
    public class PrivacyNoticeLegalBasisTab : TabBase
    {
        [UmbracoProperty("What is the legal basis for processing personal data?", "LegalBasis", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, mandatory: true,
description: "Eg your consent will be requested to process your data, a contract will be in place covering the processing of specified personal data, the Council has a legal obligation to process your data, processing is necessary for delivery of health or social care services etc. This should be documented in your Privacy Impact Assessment (PIA).", sortOrder: 1)]
        public string LegalBasis { get; set; }
    }
}