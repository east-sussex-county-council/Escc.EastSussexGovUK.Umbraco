using System;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Jobs
{
    public class JobsComponentContentTab : TabBase
    {
        /// <summary>
        /// Gets or sets the jobs service logo
        /// </summary>
        /// <value>
        /// The jobs logo.
        /// </value>
        [UmbracoProperty("Logo", "JobsLogo", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 1, Description = "Select the logo for the jobs service")]
        public string JobsLogo { get; set; }

        /// <summary>
        /// Gets or sets the background image for the header
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image", "HeaderBackgroundImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 2, Description = "Select the background image for the page header")]
        public string HeaderBackgroundImage { get; set; }

        [UmbracoProperty("Script URL", "scriptUrl", UrlDataType.PropertyEditorAlias, UrlDataType.DataTypeName, sortOrder: 3, mandatory: true, 
            Description="A standard TalentLink component is embedded into a page by referencing a script. Paste the URL here.")]
        public Uri ScriptUrl { get; set; }
    }
}