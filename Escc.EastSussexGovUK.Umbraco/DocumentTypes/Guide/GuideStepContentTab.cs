using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Guide
{
    /// <summary>
    /// Umbraco document type definition for a single step in a guide
    /// </summary>
    public class GuideStepContentTab : TabBase
    {
        [UmbracoProperty("Content", "content", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 0)]
        public string Content { get; set; }

        [UmbracoProperty("Partner images", "partnerImages", BuiltInUmbracoDataTypes.MultipleMediaPicker, sortOrder: 10)]
        public string PartnerImages { get; set; }

        [UmbracoProperty("Related links", "relatedLinks", BuiltInUmbracoDataTypes.RelatedLinks, sortOrder: 11, 
            Description="A caption with an empty link will be treated as a heading. If you do not start with a heading, a default heading will be displayed.")]
        public string RelatedLinks { get; set; }
    }
}