using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// Umbraco properties for creating one section of a privacy notice
    /// </summary>
    public class PrivacyNoticeHowLongTab : TabBase
    {
        [UmbracoProperty("How long will personal data be kept for?", "HowLong", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, mandatory: true,
description: "State how long information will be kept and/or insert a link to the ESCC retention schedule. This should be documented in your Privacy Impact Assessment (PIA).", sortOrder: 1)]
        public string HowLong { get; set; }
    }
}