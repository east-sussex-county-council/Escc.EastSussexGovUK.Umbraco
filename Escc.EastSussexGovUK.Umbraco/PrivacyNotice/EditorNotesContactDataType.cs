using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which displays hard-coded privacy notice text to editors
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesContactDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesContactDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Contact us";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p>Should you have any further queries on the uses of your information, please speak directly to our service: </p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteRenderMode",new PreValue(-1,"Html",3)}
            };
    }
}