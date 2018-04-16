using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which displays hard-coded privacy notice text to editors
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesHighStandardsDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesHighStandardsDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - High standards";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p><strong>This section will be included in the privacy notice:</strong></p>" +
                   "<p>We aim to maintain high standards, adopt best practice for our record keeping and regularly check and report " +
                    "on how we are doing.  Your information is never sold for direct marketing purposes.</p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteRenderMode",new PreValue(-1,"Html",3)}
            };
    }
}