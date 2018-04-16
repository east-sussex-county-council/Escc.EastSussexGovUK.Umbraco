using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which displays hard-coded privacy notice text to editors
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesRetentionDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesRetentionDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Retention schedule";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p>This is your retention schedule, and it should be documented in your completed Privacy Impact Assessment (PIA). " +
                   "There is <a href=\"http://intranet.escc.gov.uk/helping/dataandrecords/recordsmanagement/Pages/retentionschedules.aspx\" target=\"_blank\">retention and disposal guidance on the intranet</a>.</p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteCssClass",new PreValue(-1,"alert alert-info",3)},
               {"noteRenderMode",new PreValue(-1,"Html",4)}
            };
    }
}