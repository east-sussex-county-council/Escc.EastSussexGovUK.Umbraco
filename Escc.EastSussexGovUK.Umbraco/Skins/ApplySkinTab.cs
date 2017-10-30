using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Skins
{
    /// <summary>
    /// 'Apply skin' tab for the 'Skin' document type in Umbraco
    /// </summary>
    public class ApplySkinTab : TabBase
    {
        [UmbracoProperty("Skin", "Skin", SkinDataType.PropertyEditor, SkinDataType.DataTypeName, description: "Select the skin to apply.", sortOrder: 1)]
        public string Skin { get; set; }

        [UmbracoProperty("Where to apply it?", "WhereToApplyIt", "Umbraco.MultiNodeTreePicker", "Multi-node tree picker", description: "Select the target pages on which to apply the skin. This setting cascades to child pages.", sortOrder: 2)]
        public string ApplyToPage { get; set; }
    }
}