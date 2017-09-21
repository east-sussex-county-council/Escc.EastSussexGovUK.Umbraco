using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Content tab for <see cref="RightsOfWayDepositsDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class RightsOfWayDepositsContentTab : TabBase
    {
        [UmbracoProperty("Leading text", "leadingText", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 0)]
        public string LeadingText { get; set; }
    }
}