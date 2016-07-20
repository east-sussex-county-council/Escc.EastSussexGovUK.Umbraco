using System.Collections.Generic;
using Escc.Umbraco.PropertyEditors.App_Plugins.Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features
{
    /// <summary>
    /// A customised rich text editor data type intended to contain just a single link
    /// </summary>
    public static class RichTextSingleLinkDataType
    {
        /// <summary>
        /// The display name of the data type
        /// </summary>
        public const string DataTypeName = "Rich text: single link";

        /// <summary>
        /// Creates the data type.
        /// </summary>
        public static void CreateDataType()
        {
            var editorSettings = RichTextPropertyEditorDefaultSettings.CreateDefaultPreValues();

            var toolbar = new List<string>(editorSettings.toolbar);
            toolbar.Remove(TinyMceButtons.RemoveFormat);
            toolbar.Remove(TinyMceButtons.Bold);
            toolbar.Remove(TinyMceButtons.Numberedlist);
            toolbar.Remove(TinyMceButtons.BulletedList);
            editorSettings.toolbar = toolbar.ToArray();

            editorSettings.validators = new List<Validator>(editorSettings.validators)
            {
                new Validator(){ name = RichTextValidators.OnlyContainsLinks }
            }.ToArray();

            editorSettings.dimensions = new Dimensions()
            {
                height = 50
            };

            RichTextDataTypeService.InsertDataType(DataTypeName, editorSettings);
        }
    }
}