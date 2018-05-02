using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which displays hard-coded privacy notice text to editors
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesRightsDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesRightsDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Rights";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p><strong>This section will be included in the privacy notice:</strong></p>" +
                   "<h4>Your rights</h4>" +
                    "<p>Under data protection legislation, you have the right:</p>" +
                    "<ul>" +
                        "<li>to be informed why, where and how we use your information</li>" +
                        "<li>to ask for access to your information</li>" +
                        "<li>to ask for your information to be corrected if it is inaccurate or incomplete</li>" +
                        "<li>to ask for your information to be deleted or removed where there is no need for us to continue processing it</li>" +
                        "<li>to ask us to restrict the use of your information</li>" +
                        "<li>to ask us to copy or transfer your information from one IT system to another in a safe and secure way, without impacting the quality of the information</li>" +
                        "<li>to object to how your information is used</li>" +
                        "<li>to challenge any decisions made without human intervention (automated decision making)</li>" +
                    "</ul>" +
                    "<p>Please visit data subject rights for further details.</p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteRenderMode",new PreValue(-1,"Html",3)}
            };
    }
}