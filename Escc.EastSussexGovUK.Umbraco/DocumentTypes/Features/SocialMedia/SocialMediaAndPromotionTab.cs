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

        [UmbracoProperty("Twitter: account", "twitterAccount", BuiltInUmbracoDataTypes.Textbox, 
            description: "Don't include the @ sign. This is ignored if 'Twitter: inherit settings?' is selected.", sortOrder: 1)]
        public string TwitterAccount { get; set; }

        [UmbracoProperty("Twitter: search widget script", "twitterScript", TwitterScriptDataType.PropertyEditorAlias, TwitterScriptDataType.DataTypeName, 
            description: "Copy and paste from https://twitter.com/settings/widgets. This is ignored if 'Twitter: account' is set or 'Twitter: inherit settings?' is selected.", sortOrder: 2)]
        public string TwitterScript { get; set; }

        [UmbracoProperty("Twitter: inherit settings?", "twitterInherit", CheckboxDataType.PropertyEditorAlias, CheckboxDataType.DataTypeName, sortOrder: 3,
            description: "Inherit settings from the parent page and ignore 'Twitter: account' and 'Twitter: search widget script' set here.")]
        public string TwitterInherit { get; set; }

        [UmbracoProperty("Facebook: page URL", "facebookPageUrl", FacebookUrlDataType.PropertyEditorAlias, FacebookUrlDataType.DataTypeName, sortOrder: 4,
            description: "This is ignored if 'Facebook: inherit settings?' is selected.")]
        public Uri FacebookPageUrl { get; set; }

        [UmbracoProperty("Facebook: widget settings", "facebookWidgetSettings", BuiltInUmbracoDataTypes.CheckBoxList, "Facebook widget settings", sortOrder: 5,
            description: "This is ignored if 'Facebook: inherit settings?' is selected.")]
        public string FacebookWidgetSettings { get; set; }

        [UmbracoProperty("Facebook: inherit settings?", "facebookInherit", CheckboxDataType.PropertyEditorAlias, CheckboxDataType.DataTypeName, sortOrder: 6,
            description: "Inherit settings from the parent page and ignore 'Facebook: page URL' and 'Facebook: widget settings' set here.")]
        public string FacebookInherit { get; set; }

        [UmbracoProperty("EastSussex1Space: show widget?", "eastsussex1space", ShowWidgetDataType.PropertyEditor, ShowWidgetDataType.DataTypeName, sortOrder: 7)]
        public string EastSussex1Space { get; set; }

        [UmbracoProperty("ESCIS: show widget?", "escis", ShowWidgetDataType.PropertyEditor, ShowWidgetDataType.DataTypeName, sortOrder: 8)]
        public string Escis { get; set; }
    }
}