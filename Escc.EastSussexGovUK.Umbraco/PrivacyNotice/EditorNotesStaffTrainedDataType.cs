using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which displays hard-coded privacy notice text to editors
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesStaffTrainedDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesStaffTrainedDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Staff trained";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p>Our staff are trained to handle your information correctly and protect your confidentiality and privacy.</p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteRenderMode",new PreValue(-1,"Html",3)}
            };
    }
}