using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which displays hard-coded privacy notice text to editors
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesComplainDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesComplainDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Complain";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p><strong>This section will be included in the privacy notice:</strong></p>" +
                   "<p>To complain about the use of your information, please contact our <a href=\"https://www.eastsussex.gov.uk/contact-us/complaints/corporate-complaints/\" target=\"_blank\">Customer Services Team</a> " +
                    "or our <a href=\"https://www.eastsussex.gov.uk/yourcouncil/about/keydocuments/foi/dataprotection/data-protection-officer/\" target=\"_blank\">Data Protection Officer</a>.</p>" +

                "<p><a href=\"https://www.eastsussex.gov.uk/contact-us/complaints\" target=\"_blank\">Further information on making a complaint</a>.</p>" +

                "<p>You can also contact the <abbr title=\"Information Commissioner's Office\">ICO</abbr> for further information or to make a complaint:</p>" +

                "<p>Information Commissioner's Office<br />" +
                    "Wycliffe House<br />" +
                    "Water Lane<br />" +
                    "Wilmslow<br />" +
                    "Cheshire SK9 5AF</p>" +

                "<p>Phone: 0303 123 1113 (local rate) or 01625 545 745 if you prefer to use a national rate number.</p>" +
                "<p><a href=\"https://ico.org.uk/global/contact-us/email/\" target=\"_blank\">Email ICO</a></p>" +
                "<p><a href=\"https://ico.org.uk/concerns/\" target=\"_blank\">Report a concern on the ICO website</a></p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteRenderMode",new PreValue(-1,"Html",3)}
            };
    }
}