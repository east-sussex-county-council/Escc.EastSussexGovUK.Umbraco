using System;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Jobs
{
    public class JobsComponentContentTab : TabBase
    {
        [UmbracoProperty("Script URL", "scriptUrl", UrlDataType.PropertyEditorAlias, UrlDataType.DataTypeName, sortOrder: 1, mandatory: true, 
            Description="A standard TalentLink component is embedded into a page by referencing a script. Paste the URL here.")]
        public Uri ScriptUrl { get; set; }
    }
}