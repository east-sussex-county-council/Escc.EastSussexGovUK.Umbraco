using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Forms
{
    public class FormContentTab : TabBase
    {
        [UmbracoProperty("Leading text", "LeadingText", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 0,
            Description = "This will appear at the top of every page of the form.")]
        public string LeadingText { get; set; }

        [UmbracoProperty("Form", FormDataType.DataTypeName, FormDataType.PropertyEditor, sortOrder: 1, Mandatory = true)]
        public string Form { get; set; }
    }
}