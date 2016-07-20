using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.App_Plugins.Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features
{
    /// <summary>
    /// A customised rich text editor data type which validates that it contains only links, and formats the links into two unordered lists
    /// </summary>
    public static class RichTextTwoListsOfLinksDataType
    {
        /// <summary>
        /// The display name of the data type
        /// </summary>
        public const string DataTypeName = "Rich text: two lists of links";

        /// <summary>
        /// Creates the data type.
        /// </summary>
        public static void CreateDataType()
        {
            var editorSettings = RichTextPropertyEditorDefaultSettings.CreateDefaultPreValues();
            editorSettings.validators = new List<Validator>(editorSettings.validators)
            {
                new Validator() { name = RichTextValidators.OnlyContainsLinks }
            }.ToArray();
            editorSettings.formatters = new List<string>(editorSettings.formatters)
            {
                RichTextFormatters.FormatContentAsTwoListsOfLinks
            }.ToArray();

            RichTextDataTypeService.InsertDataType(DataTypeName, editorSettings);
        }
    }
}