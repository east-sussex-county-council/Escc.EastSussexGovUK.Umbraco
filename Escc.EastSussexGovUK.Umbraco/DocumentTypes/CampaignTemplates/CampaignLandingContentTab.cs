using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates
{
    /// <summary>
    /// Content tab for the campaign landing document type
    /// </summary>
    public class CampaignLandingContentTab : TabBase
    {
        [UmbracoProperty("Introduction", "Introduction", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccStandardDataType.DataTypeName, sortOrder: 2,
            Description="Large text which appears at the top of the page."
            )]
        public string Introduction { get; set; }

        [UmbracoProperty("Landing navigation", "LandingNavigation", BuiltInUmbracoDataTypes.RelatedLinks, sortOrder: 3,
            Description= "Tiled links similar to a landing page.")]
        public string LandingNavigation { get; set; }

        [UmbracoProperty("Button navigation", "ButtonNavigation", BuiltInUmbracoDataTypes.RelatedLinks, sortOrder: 4,
            Description = "Up to three buttons, which will be displayed with the descriptions below.")]
        public string ButtonNavigation { get; set; }

        [UmbracoProperty("Button 1 description", "Button1Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 5)]
        public string Button1Description { get; set; }

        [UmbracoProperty("Button 2 description", "Button2Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 7)]
        public string Button2Description { get; set; }

        [UmbracoProperty("Button 3 description", "Button3Description", BuiltInUmbracoDataTypes.Textbox, sortOrder: 9)]
        public string Button3Description { get; set; }

        [UmbracoProperty("Content", "Content", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 10, 
            Description = "The content which should appear below the buttons. You can embed YouTube videos here.")]
        public string Content { get; set; }
    }
}