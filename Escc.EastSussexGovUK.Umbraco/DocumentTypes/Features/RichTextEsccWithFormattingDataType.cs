using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.App_Plugins.Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features
{
    /// <summary>
    /// A customised rich text editor data type which allows formatting and embedding
    /// </summary>
    public static class RichTextEsccWithFormattingDataType
    {
        /// <summary>
        /// The display name of the data type
        /// </summary>
        public const string DataTypeName = "Rich text: ESCC with formatting";

        /// <summary>
        /// Creates the data type.
        /// </summary>
        public static void CreateDataType()
        {
            var editorSettings = RichTextPropertyEditorDefaultSettings.CreateDefaultPreValues();

            editorSettings.toolbar = new List<string>(editorSettings.toolbar)
            {
                TinyMceButtons.StyleSelect,
                TinyMceButtons.Table,
                TinyMceButtons.Blockquote
            }.ToArray();

            editorSettings.stylesheets = new List<string>(editorSettings.stylesheets)
            {
                RichTextPropertyEditorDefaultSettings.StylesheetHeadings,
                "TinyMCE-StyleSelector-Embed"
            }.ToArray();

            var validators = new List<Validator>(editorSettings.validators);
            validators.RemoveAll(val => val.name == RichTextValidators.DoNotLinkToDocuments);
            editorSettings.validators = validators.ToArray();

            RichTextDataTypeService.InsertDataType(DataTypeName, editorSettings);
        }
    }
}