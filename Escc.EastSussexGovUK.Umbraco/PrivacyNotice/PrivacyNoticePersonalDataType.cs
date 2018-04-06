using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which explains personal data
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(PrivacyNoticePersonalDataType), DataTypeDatabaseType.Nvarchar)]
    public class PrivacyNoticePersonalDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Personal data";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<h4>Personal data</h4>" +
"<p>Any personally identifiable information relating to a living individual, eg name, identification number, location data etc.</p>" +
"<h4>Personal data (special category)</h4>" +
"<p>Personal data that reveals racial or ethnic origin, political opinions, religious or philosophical beliefs, trade union membership, genetic data, biometric data (for the purpose of uniquely identifying a natural person), data concerning health or data concerning a person’s sex life or sexual orientation.</p>",1)},
               {"hideLabel",new PreValue(-1,"0",2)},
               {"noteRenderMode",new PreValue(-1,"Html",3)}
            };
    }
}