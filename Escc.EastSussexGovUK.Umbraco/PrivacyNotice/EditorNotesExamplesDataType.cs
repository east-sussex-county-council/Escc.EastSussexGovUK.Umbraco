using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.PrivacyNotice
{
    /// <summary>
    /// An Umbraco editor notes data type which links to examples of privacy notices
    /// </summary>
    [UmbracoDataType(DataTypeName, PropertyEditor, typeof(EditorNotesExamplesDataType), DataTypeDatabaseType.Nvarchar)]
    public class EditorNotesExamplesDataType : IPreValueProvider
    {
        public const string DataTypeName = "Privacy notice - Examples";
        public const string PropertyEditor = "tooorangey.EditorNotes";

        public IDictionary<string, PreValue> PreValues { get; } = new Dictionary<string, PreValue>()
            {
               {"editorNotes",new PreValue(-1,"<p>See examples of existing <a href=\"https://www.eastsussex.gov.uk/privacy/\" target=\"_blank\">Privacy notices</a>.</p>",1)},
               {"hideLabel",new PreValue(-1,"1",2)},
               {"noteCssClass",new PreValue(-1,"alert alert-info",3)},
               {"noteRenderMode",new PreValue(-1,"Html",4)}
            };
    }
}