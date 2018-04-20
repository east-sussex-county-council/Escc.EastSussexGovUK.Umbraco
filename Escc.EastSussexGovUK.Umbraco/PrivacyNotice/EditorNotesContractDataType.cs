using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which displays hard-coded privacy notice text to editors
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesContractDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesContractDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Contractual obligation";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p><strong>This section will be included in the privacy notice:</strong></p>" +
                   "<p>Any organisation commissioned by the Council will be under contractual obligation to comply with data protection legislation.</p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteRenderMode",new PreValue(-1,"Html",3)}
            };
    }
}