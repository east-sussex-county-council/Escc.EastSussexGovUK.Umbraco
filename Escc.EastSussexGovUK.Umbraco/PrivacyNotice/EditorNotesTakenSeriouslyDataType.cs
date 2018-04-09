using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which displays hard-coded privacy notice text to editors
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesTakenSeriouslyDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesTakenSeriouslyDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Data protection taken seriously";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p>" +
                    "East Sussex County Council takes data protection seriously. Please be assured that your information will be used " +
                    "appropriately in line with data protection legislation, will be stored securely and will not be processed unless " +
                    "the requirements for fair and lawful processing can be met." +
                "</p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteRenderMode",new PreValue(-1,"Html",3)}
            };
    }
}