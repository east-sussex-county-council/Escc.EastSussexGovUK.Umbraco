using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates
{
    /// <summary>
    /// Content tab for the campaign content document type
    /// </summary>
    public class CampaignContentContentTab : TabBase
    {
        [UmbracoProperty("Content part 1", "Content1", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 1,
            Description="Text which appears above the upper pull quote."
            )]
        public string Content1 { get; set; }

        [UmbracoProperty("Upper pull quote", "UpperQuote", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 2,
            Description="Appears within the text on small screens, and on the right on larger screens. Don't put this too high up, to avoid clutter on small screens."
            )]
        public string UpperQuote { get; set; }

        [UmbracoProperty("Upper image", "UpperImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 3,
            Description = "Appears within the text on small screens, and on the right on larger screens. Don't put this too high up, to avoid clutter on small screens.")]
        public string UpperImage { get; set; }

        [UmbracoProperty("Content part 2", "Content2", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 4,
            Description = "Text which appears between the upper and central pull quotes."
            )]
        public string Content2 { get; set; }

        [UmbracoProperty("Central pull quote", "CentralQuote", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 5,
            Description = "Appears within the text on small screens, and the image extends into the right column on larger screens."
            )]
        public string CentralQuote { get; set; }

        [UmbracoProperty("Central pull quote image", "CentralQuoteImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 6,
            Description = "A 310px wide cutout image of the speaker for the central pull quote.")]
        public string CentralQuoteImage { get; set; }

        [UmbracoProperty("Content part 3", "Content3", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 7,
                    Description = "Text which appears between the central and lower pull quotes."
                    )]
        public string Content3 { get; set; }

        [UmbracoProperty("Lower pull quote", "LowerQuote", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 8,
            Description="Appears within the text on small screens, and on the right on larger screens."
            )]
        public string LowerQuote { get; set; }

        [UmbracoProperty("Lower image", "LowerImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 9,
            Description = "Appears within the text on small screens, and on the right on larger screens.")]
        public string LowerImage { get; set; }

        [UmbracoProperty("Content part 4", "Content4", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 10,
            Description = "Text which appears between the lower and the final pull quote."
            )]
        public string Content4 { get; set; }

        [UmbracoProperty("Content part 5", "Content5", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 11,
        Description = "Text which appears alongside the final pull quote image on medium screens."
        )]
        public string Content5 { get; set; }

        [UmbracoProperty("Content part 6", "Content6", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 12,
        Description = "Text which appears alongside the final pull quote image on small and medium screens."
        )]
        public string Content6 { get; set; }

        [UmbracoProperty("Final pull quote", "FinalQuote", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 13,
            Description="Appears at the end of the text together with an image."
            )]
        public string BottomQuote { get; set; }

        [UmbracoProperty("Final pull quote image", "FinalQuoteImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 14,
            Description = "An 310px wide cutout image of the speaker for the final pull quote.")]
        public string BottomQuoteImage { get; set; }

        [UmbracoProperty("Final pull quote attribution", "FinalQuoteAttribution", BuiltInUmbracoDataTypes.Textbox, sortOrder: 15,
            Description = "The name of the speaker in the final pull quote."
            )]
        public string BottomQuoteAttribution { get; set; }
    }
}