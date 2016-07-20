using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.App_Plugins.Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.Latest
{
    /// <summary>
    /// A customised rich text editor data type for 'latest' text
    /// </summary>
    public static class LatestDataType
    {
        /// <summary>
        /// The display name of the data type
        /// </summary>
        public const string DataTypeName = "Latest";

        /// <summary>
        /// Creates the data type.
        /// </summary>
        public static void CreateDataType()
        {
            var editorSettings = RichTextPropertyEditorDefaultSettings.CreateDefaultPreValues();
            editorSettings.validators = new List<Validator>(editorSettings.validators)
            {
                new Validator() { name = RichTextValidators.MaximumWordCount, max = 100 }
            }.ToArray();

            RichTextDataTypeService.InsertDataType(DataTypeName, editorSettings);
        }
    }
}