using System;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.ServiceAlerts
{
    /// <summary>
    /// Content tab for the 'Alert' document type in Umbraco
    /// </summary>
    public class AlertContentTab : TabBase
    {
        [UmbracoProperty("Alert", "alert", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccStandardDataType.DataTypeName, sortOrder: 0)]
        public string Alert { get; set; }

        [UmbracoProperty("Where to display it?", "whereToDisplayIt", "Umbraco.MultiNodeTreePicker", "Multi-node tree picker", description: "Select the target pages on which to display the alert", sortOrder: 1)]
        public string WhereToDisplayIt { get; set; }

        [UmbracoProperty("Where else to display it?", "whereElseToDisplayIt", BuiltInUmbracoDataTypes.TextboxMultiple, description: "Paste any target page URLs, one per line. This is useful for targeting external applications like the online library catalogue.", sortOrder: 2)]
        public string WhereElseToDisplayIt { get; set; }

        [UmbracoProperty("Add to alerts from parent pages", "append", CheckboxDataType.PropertyEditorAlias, CheckboxDataType.DataTypeName, description: "Adds this alert to those from parent pages rather than replacing them", sortOrder: 3)]
        public string Append { get; set; }

        [UmbracoProperty("Cascade", "cascade", CheckboxDataType.PropertyEditorAlias, CheckboxDataType.DataTypeName, description: "Allow this alert to appear on child pages", sortOrder: 4)]
        public DateTime Cascade { get; set; }
    }
}
