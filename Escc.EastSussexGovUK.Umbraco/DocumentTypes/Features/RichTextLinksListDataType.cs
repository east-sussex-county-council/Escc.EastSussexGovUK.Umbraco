using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.App_Plugins.Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features
{
    /// <summary>
    /// A customised rich text editor data type which validates that it contains only links
    /// </summary>
    public static class RichTextLinksListDataType
    {
        /// <summary>
        /// The display name of the data type
        /// </summary>
        public const string DataTypeName = "Rich text: links list";

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

            RichTextDataTypeService.InsertDataType(DataTypeName, editorSettings);
        }
    }
}