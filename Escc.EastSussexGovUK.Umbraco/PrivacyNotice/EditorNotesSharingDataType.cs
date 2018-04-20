using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which displays hard-coded privacy notice text to editors
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesSharingDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesSharingDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Sharing";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p><strong>This section will be included in the privacy notice:</strong></p>" +
                   "<p>Any sharing of personal data is always done:</p>" +
                "<ul>" +
                    "<li>on case-by-case basis</li>" +
                    "<li>using the minimum personal data necessary</li>" +
                    "<li>with the appropriate security controls in place</li>" +
                    "<li>in line with legislation.</li>" +
                "</ul>" +
                "<p>Information is only shared with those agencies and bodies who have a \"need to know\" or where you have consented to " +
                    "the sharing of your personal data to such persons.</p>" +
                "<p>We may use the information we hold about you to assist in the detection and prevention of crime or fraud. " +
                    "We may also share this information with other bodies that inspect and manage public funds.</p>" +
                "<p>We will not routinely disclose any information about you without your express permission. However, there are " +
                    "circumstances where we must or can share information about you owing to a legal or statutory obligation.</p>" +
                   "<h4><strong>Use of third party organisations</strong></h4>" +
                   "<p>East Sussex County Council may share your information with trusted external organisations to process your data on our behalf.</p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteRenderMode",new PreValue(-1,"Html",3)}
            };
    }
}