using System;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.SocialMedia
{
    /// <summary>
    /// Social media and promotion tab shared by several groups of document types
    /// </summary>
    public class SocialMediaAndPromotionTab : TabBase
    {
        [UmbracoProperty("Social media order", "socialMediaOrder", BuiltInUmbracoDataTypes.RadioButtonList, "Social media order", sortOrder: 0)]
        public string SocialMediaOrder { get; set; }

        [UmbracoProperty("Twitter: account", "twitterAccount", BuiltInUmbracoDataTypes.Textbox, description: "Don't include the @ sign", sortOrder: 1)]
        public string TwitterAccount { get; set; }

        [UmbracoProperty("Twitter: paste widget script", "twitterScript", TwitterScriptDataType.PropertyEditorAlias, TwitterScriptDataType.DataTypeName, description: "For backwards compatibility only. Use 'Twitter: account' instead.", sortOrder: 2)]
        [Obsolete]
        public string TwitterScript { get; set; }

        [UmbracoProperty("Twitter: inherit widget?", "twitterInherit", CheckboxDataType.PropertyEditorAlias, CheckboxDataType.DataTypeName, sortOrder: 3)]
        public string TwitterInherit { get; set; }

        [UmbracoProperty("Facebook: page URL", "facebookPageUrl", FacebookUrlDataType.PropertyEditorAlias, FacebookUrlDataType.DataTypeName, sortOrder: 4)]
        public Uri FacebookPageUrl { get; set; }

        [UmbracoProperty("Facebook: widget settings", "facebookWidgetSettings", BuiltInUmbracoDataTypes.CheckBoxList, "Facebook widget settings", sortOrder: 5)]
        public string FacebookWidgetSettings { get; set; }

        [UmbracoProperty("Facebook: inherit widget?", "facebookInherit", CheckboxDataType.PropertyEditorAlias, CheckboxDataType.DataTypeName, sortOrder: 6)]
        public string FacebookInherit { get; set; }

        [UmbracoProperty("EastSussex1Space: show widget?", "eastsussex1space", ShowWidgetDataType.PropertyEditor, ShowWidgetDataType.DataTypeName, sortOrder: 7)]
        public string EastSussex1Space { get; set; }

        [UmbracoProperty("ESCIS: show widget?", "escis", ShowWidgetDataType.PropertyEditor, ShowWidgetDataType.DataTypeName, sortOrder: 8)]
        public string Escis { get; set; }
    }
}