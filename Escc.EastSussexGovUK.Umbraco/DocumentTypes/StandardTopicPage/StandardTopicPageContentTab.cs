using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage
{
    public class StandardTopicPageContentTab : TabBase
    {
        [UmbracoProperty("Section navigation", "highlightContent", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextTwoListsOfLinksDataType.DataTypeName, description: "Add links to other pages in this section. Rarely needed.", sortOrder: 1)]
        public string HighlightContent { get; set; }

        [UmbracoProperty("Section navigation image", "highlightImage", BuiltInUmbracoDataTypes.MediaPicker, description: "Image next to links to other pages in this section. Rarely needed.", sortOrder: 2)]
        public string HighlightImage { get; set; }

        [UmbracoProperty("Introductory text", "phDefIntro", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 3)]
        public string PhDefIntro { get; set; }

        /*------------Section 1------------*/

        [UmbracoProperty("Section 1: layout", "phDefSection01", BuiltInUmbracoDataTypes.DropDown, TopicPageLayoutDataType.DataTypeName, sortOrder: 4)]
        public string PhDefSection01 { get; set; }

        [UmbracoProperty("Section 1: image 1", "phDefImage01", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 5)]
        public string PhDefImage01 { get; set; }

        [UmbracoProperty("Caption for image 1", "phDefCaption01", BuiltInUmbracoDataTypes.Textbox, sortOrder: 6)]
        public string PhDefCaption01 { get; set; }

        [UmbracoProperty("Use image 1's alt text as its caption", "phDefAltAsCaption01", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 7)] 
        public string PhDefAltAsCaption01 { get; set; }

        [UmbracoProperty("Section 1: image 2", "phDefImage04", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 8)]
        public string PhDefImage04 { get; set; }

        [UmbracoProperty("Caption for image 2", "phDefCaption04", BuiltInUmbracoDataTypes.Textbox, sortOrder: 9)]
        public string PhDefCaption04 { get; set; }

        [UmbracoProperty("Use image 2's alt text as its caption", "phDefAltAsCaption04", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 10)]
        public string PhDefAltAsCaption04 { get; set; }

        [UmbracoProperty("Section 1: image 3", "phDefImage05", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 11)]
        public string PhDefImage05 { get; set; }

        [UmbracoProperty("Caption for image 3", "phDefCaption05", BuiltInUmbracoDataTypes.Textbox, sortOrder: 12)]
        public string PhDefCaption05 { get; set; }

        [UmbracoProperty("Use image 3's alt text as its caption", "phDefAltAsCaption05", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 13)]
        public string PhDefAltAsCaption05 { get; set; }

        [UmbracoProperty("Section 1: subtitle", "phDefSubtitle01", BuiltInUmbracoDataTypes.Textbox, sortOrder: 14)]
        public string PhDefSubtitle01 { get; set; }

        [UmbracoProperty("Section 1: content", "phDefContent01", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 15)]
        public string PhDefContent01 { get; set; }

        /*------------Section 2------------*/

        [UmbracoProperty("Section 2: layout", "phDefSection02", BuiltInUmbracoDataTypes.DropDown, TopicPageLayoutDataType.DataTypeName, sortOrder: 16)] 
        public string PhDefSection02 { get; set; }

        [UmbracoProperty("Section 2: image 1", "phDefTopicImageNoWrap01", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 17)] 
        public string PhDefTopicImageNoWrap01 { get; set; }

        [UmbracoProperty("Caption for image 1", "phDefCaption14", BuiltInUmbracoDataTypes.Textbox, sortOrder: 18)]
        public string PhDefCaption14 { get; set; }

        [UmbracoProperty("Use image 1's alt text as its caption", "phDefAltAsCaption14", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 19)]
        public string PhDefAltAsCaption14 { get; set; }

        [UmbracoProperty("Section 2: image 2", "phDefImage06", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 20)]
        public string PhDefImage06 { get; set; }

        [UmbracoProperty("Caption for image 2", "phDefCaption06", BuiltInUmbracoDataTypes.Textbox, sortOrder: 21)]
        public string PhDefCaption06 { get; set; }

        [UmbracoProperty("Use image 2's alt text as its caption", "phDefAltAsCaption06", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 22)]
        public string PhDefAltAsCaption06 { get; set; }

        [UmbracoProperty("Section 2: image 3", "phDefImage07", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 23)]
        public string PhDefImage07 { get; set; }

        [UmbracoProperty("Caption for image 3", "phDefCaption07", BuiltInUmbracoDataTypes.Textbox, sortOrder: 24)]
        public string PhDefCaption07 { get; set; }

        [UmbracoProperty("Use image 3's alt text as its caption", "phDefAltAsCaption07", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 25)]
        public string PhDefAltAsCaption07 { get; set; }

        [UmbracoProperty("Section 2: subtitle", "phDefSubtitle02", BuiltInUmbracoDataTypes.Textbox, sortOrder: 26)]
        public string PhDefSubtitle02 { get; set; }

        [UmbracoProperty("Section 2: content", "phDefContent02", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 27)]
        public string PhDefContent02 { get; set; }

        /*------------Section 3------------*/

        [UmbracoProperty("Section 3: layout", "phDefSection03", BuiltInUmbracoDataTypes.DropDown, TopicPageLayoutDataType.DataTypeName, sortOrder: 28)] 
        public string PhDefSection03 { get; set; }

        [UmbracoProperty("Section 3: image 1", "phDefImage02", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 29)]
        public string PhDefImage02 { get; set; }

        [UmbracoProperty("Caption for image 1", "phDefCaption02", BuiltInUmbracoDataTypes.Textbox, sortOrder: 30)]
        public string PhDefCaption02 { get; set; }

        [UmbracoProperty("Use image 1's alt text as its caption", "phDefAltAsCaption02", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 31)]
        public string PhDefAltAsCaption02 { get; set; }

        [UmbracoProperty("Section 3: image 2", "phDefImage08", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 32)]
        public string PhDefImage08 { get; set; }

        [UmbracoProperty("Caption for image 2", "phDefCaption08", BuiltInUmbracoDataTypes.Textbox, sortOrder: 33)]
        public string PhDefCaption08 { get; set; }

        [UmbracoProperty("Use image 2's alt text as its caption", "phDefAltAsCaption08", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 34)]
        public string PhDefAltAsCaption08 { get; set; }

        [UmbracoProperty("Section 3: image 3", "phDefImage09", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 35)]
        public string PhDefImage09 { get; set; }

        [UmbracoProperty("Caption for image 3", "phDefCaption09", BuiltInUmbracoDataTypes.Textbox, sortOrder: 36)]
        public string PhDefCaption09 { get; set; }

        [UmbracoProperty("Use image 3's alt text as its caption", "phDefAltAsCaption09", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 37)]
        public string PhDefAltAsCaption09 { get; set; }

        [UmbracoProperty("Section 3: subtitle", "phDefSubtitle03", BuiltInUmbracoDataTypes.Textbox, sortOrder: 38)]
        public string PhDefSubtitle03 { get; set; }

        [UmbracoProperty("Section 3: content", "phDefContent03", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 39)]
        public string PhDefContent03 { get; set; }

        /*------------Section 4------------*/

        [UmbracoProperty("Section 4: layout", "phDefSection04", BuiltInUmbracoDataTypes.DropDown, TopicPageLayoutDataType.DataTypeName, sortOrder: 40)] 
        public string PhDefSection04 { get; set; }

        [UmbracoProperty("Section 4: image 1", "phDefImage03", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 41)]
        public string PhDefImage03 { get; set; }

        [UmbracoProperty("Caption for image 1", "phDefCaption03", BuiltInUmbracoDataTypes.Textbox, sortOrder: 42)]
        public string PhDefCaption03 { get; set; }

        [UmbracoProperty("Use image 1's alt text as its caption", "phDefAltAsCaption03", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 43)]
        public string PhDefAltAsCaption03 { get; set; }

        [UmbracoProperty("Section 4: image 2", "phDefImage10", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 44)]
        public string PhDefImage10 { get; set; }

        [UmbracoProperty("Caption for image 2", "phDefCaption10", BuiltInUmbracoDataTypes.Textbox, sortOrder: 45)]
        public string PhDefCaption10 { get; set; }

        [UmbracoProperty("Use image 2's alt text as its caption", "phDefAltAsCaption10", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 46)]
        public string PhDefAltAsCaption10 { get; set; }

        [UmbracoProperty("Section 4: image 3", "phDefImage11", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 47)]
        public string PhDefImage11 { get; set; }

        [UmbracoProperty("Caption for image 3", "phDefCaption11", BuiltInUmbracoDataTypes.Textbox, sortOrder: 48)]
        public string PhDefCaption11 { get; set; }

        [UmbracoProperty("Use image 3's alt text as its caption", "phDefAltAsCaption11", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 49)]
        public string PhDefAltAsCaption11 { get; set; }

        [UmbracoProperty("Section 4: subtitle", "phDefSubtitle04", BuiltInUmbracoDataTypes.Textbox, sortOrder: 50)]
        public string PhDefSubtitle04 { get; set; }

        [UmbracoProperty("Section 4: content", "phDefContent04", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 51)]
        public string PhDefContent04 { get; set; }

        /*------------Section 5------------*/

        [UmbracoProperty("Section 5: layout", "phDefSection05", BuiltInUmbracoDataTypes.DropDown, TopicPageLayoutDataType.DataTypeName, sortOrder: 52)] 
        public string PhDefSection05 { get; set; }

        [UmbracoProperty("Section 5: image 1", "phDefTopicImageNoWrap02", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 53)]
        public string PhDefTopicImageNoWrap02 { get; set; }

        [UmbracoProperty("Caption for image 1", "phDefCaption15", BuiltInUmbracoDataTypes.Textbox, sortOrder: 54)]
        public string PhDefCaption15 { get; set; }

        [UmbracoProperty("Use image 1's alt text as its caption", "phDefAltAsCaption15", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 55)]
        public string PhDefAltAsCaption15 { get; set; }

        [UmbracoProperty("Section 5: image 2", "phDefImage12", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 56)]
        public string PhDefImage12 { get; set; }

        [UmbracoProperty("Caption for image 2", "phDefCaption12", BuiltInUmbracoDataTypes.Textbox, sortOrder: 57)]
        public string PhDefCaption12 { get; set; }

        [UmbracoProperty("Use image 2's alt text as its caption", "phDefAltAsCaption12", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 58)]
        public string PhDefAltAsCaption12 { get; set; }

        [UmbracoProperty("Section 5: image 3", "phDefImage13", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 59)]
        public string PhDefImage13 { get; set; }

        [UmbracoProperty("Caption for image 3", "phDefCaption13", BuiltInUmbracoDataTypes.Textbox, sortOrder: 60)]
        public string PhDefCaption13 { get; set; }

        [UmbracoProperty("Use image 3's alt text as its caption", "phDefAltAsCaption13", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 61)]
        public string PhDefAltAsCaption13 { get; set; }

        [UmbracoProperty("Section 5: subtitle", "phDefSubtitle05", BuiltInUmbracoDataTypes.Textbox, sortOrder: 62)]
        public string PhDefSubtitle05 { get; set; }

        [UmbracoProperty("Section 5: content", "phDefContent05", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 63)]
        public string PhDefContent05 { get; set; }

        /*------------Section 6------------*/

        [UmbracoProperty("Section 6: layout", "phDefSection06", BuiltInUmbracoDataTypes.DropDown, TopicPageLayoutDataType.DataTypeName, sortOrder: 64)] 
        public string PhDefSection06 { get; set; }

        [UmbracoProperty("Section 6: image 1", "phDefImage16", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 65)]
        public string PhDefImage16 { get; set; }

        [UmbracoProperty("Caption for image 1", "phDefCaption16", BuiltInUmbracoDataTypes.Textbox, sortOrder: 66)]
        public string PhDefCaption16 { get; set; }

        [UmbracoProperty("Use image 1's alt text as its caption", "phDefAltAsCaption16", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 67)]
        public string PhDefAltAsCaption16 { get; set; }

        [UmbracoProperty("Section 6: image 2", "phDefImage17", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 68)]
        public string PhDefImage17 { get; set; }

        [UmbracoProperty("Caption for image 2", "phDefCaption17", BuiltInUmbracoDataTypes.Textbox, sortOrder: 69)]
        public string PhDefCaption17 { get; set; }

        [UmbracoProperty("Use image 2's alt text as its caption", "phDefAltAsCaption17", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 70)]
        public string PhDefAltAsCaption17 { get; set; }

        [UmbracoProperty("Section 6: image 3", "phDefImage18", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 71)]
        public string PhDefImage18 { get; set; }

        [UmbracoProperty("Caption for image 3", "phDefCaption18", BuiltInUmbracoDataTypes.Textbox, sortOrder: 72)]
        public string PhDefCaption18 { get; set; }

        [UmbracoProperty("Use image 3's alt text as its caption", "phDefAltAsCaption18", PropertyEditorAliases.BooleanPropertyEditor, CheckboxDataType.DataTypeName, sortOrder: 73)]
        public string PhDefAltAsCaption18 { get; set; }

        [UmbracoProperty("Section 6: subtitle", "phDefSubtitle06", BuiltInUmbracoDataTypes.Textbox, sortOrder: 74)]
        public string PhDefSubtitle06 { get; set; }

        [UmbracoProperty("Section 6: content", "phDefContent06", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 75)]
        public string PhDefContent06 { get; set; }


        /*------------Other------------*/

        [UmbracoProperty("Section 7: subtitle", "phDefSubtitle07", BuiltInUmbracoDataTypes.Textbox, sortOrder: 76)]
        public string PhDefSubtitle07 { get; set; }

        [UmbracoProperty("Section 7: content", "phDefContent07", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 77)]
        public string PhDefContent07 { get; set; }

        [UmbracoProperty("Section 8: subtitle", "phDefSubtitle08", BuiltInUmbracoDataTypes.Textbox, sortOrder: 78)]
        public string PhDefSubtitle08 { get; set; }

        [UmbracoProperty("Section 8: content", "phDefContent08", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 79)]
        public string PhDefContent08 { get; set; }

        [UmbracoProperty("Section 9: subtitle", "phDefSubtitle09", BuiltInUmbracoDataTypes.Textbox, sortOrder: 80)]
        public string PhDefSubtitle09 { get; set; }

        [UmbracoProperty("Section 9: content", "phDefContent09", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 81)]
        public string PhDefContent09 { get; set; }

        [UmbracoProperty("Section 10: subtitle", "phDefSubtitle10", BuiltInUmbracoDataTypes.Textbox, sortOrder: 82)]
        public string PhDefSubtitle10 { get; set; }

        [UmbracoProperty("Section 10: content", "phDefContent10", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 83)]
        public string PhDefContent10 { get; set; }

        [UmbracoProperty("Section 11: subtitle", "phDefSubtitle11", BuiltInUmbracoDataTypes.Textbox, sortOrder: 84)]
        public string PhDefSubtitle11 { get; set; }

        [UmbracoProperty("Section 11: content", "phDefContent11", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 85)]
        public string PhDefContent11 { get; set; }

        [UmbracoProperty("Section 12: subtitle", "phDefSubtitle12", BuiltInUmbracoDataTypes.Textbox, sortOrder: 86)]
        public string PhDefSubtitle12 { get; set; }

        [UmbracoProperty("Section 12: content", "phDefContent12", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 87)]
        public string PhDefContent12 { get; set; }

        [UmbracoProperty("Section 13: subtitle", "phDefSubtitle13", BuiltInUmbracoDataTypes.Textbox, sortOrder: 88)]
        public string PhDefSubtitle13 { get; set; }

        [UmbracoProperty("Section 13: content", "phDefContent13", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 89)]
        public string PhDefContent13 { get; set; }

        [UmbracoProperty("Section 14: subtitle", "phDefSubtitle14", BuiltInUmbracoDataTypes.Textbox, sortOrder: 90)]
        public string PhDefSubtitle14 { get; set; }

        [UmbracoProperty("Section 14: content", "phDefContent14", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 91)]
        public string PhDefContent14 { get; set; }

        [UmbracoProperty("Section 15: subtitle", "phDefSubtitle15", BuiltInUmbracoDataTypes.Textbox, sortOrder: 92)]
        public string PhDefSubtitle15 { get; set; }

        [UmbracoProperty("Section 15: content", "phDefContent15", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 93)]
        public string PhDefContent15 { get; set; }

        [UmbracoProperty("Section 16: subtitle", "phDefSubtitle16", BuiltInUmbracoDataTypes.Textbox, sortOrder: 94)]
        public string PhDefSubtitle16 { get; set; }

        [UmbracoProperty("Section 16: content", "phDefContent16", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 95)]
        public string PhDefContent16 { get; set; }

        [UmbracoProperty("Section 17: subtitle", "phDefSubtitle17", BuiltInUmbracoDataTypes.Textbox, sortOrder: 96)]
        public string PhDefSubtitle17 { get; set; }

        [UmbracoProperty("Section 17: content", "phDefContent17", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 97)]
        public string PhDefContent17 { get; set; }

        [UmbracoProperty("Section 18: subtitle", "phDefSubtitle18", BuiltInUmbracoDataTypes.Textbox, sortOrder: 98)]
        public string PhDefSubtitle18 { get; set; }

        [UmbracoProperty("Section 18: content", "phDefContent18", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 99)]
        public string PhDefContent18 { get; set; }

        [UmbracoProperty("Section 19: subtitle", "phDefSubtitle19", BuiltInUmbracoDataTypes.Textbox, sortOrder: 100)]
        public string PhDefSubtitle19 { get; set; }

        [UmbracoProperty("Section 19: content", "phDefContent19", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 101)]
        public string PhDefContent19 { get; set; }

        [UmbracoProperty("Section 20: subtitle", "phDefSubtitle20", BuiltInUmbracoDataTypes.Textbox, sortOrder: 102)]
        public string PhDefSubtitle20 { get; set; }

        [UmbracoProperty("Section 20: content", "phDefContent20", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 103)]
        public string PhDefContent20 { get; set; }

        [UmbracoProperty("Logo 1", "phDefLogo01", BuiltInUmbracoDataTypes.MediaPicker, description: "If this content requires partner logos, add them here. They should be small.", sortOrder: 104)] 
        public string PhDefLogo01 { get; set; }

        [UmbracoProperty("Logo 2", "phDefLogo02", BuiltInUmbracoDataTypes.MediaPicker, description: "If this content requires partner logos, add them here. They should be small.", sortOrder: 105)] 
        public string PhDefLogo02 { get; set; }

        [UmbracoProperty("Logo 3", "phDefLogo03", BuiltInUmbracoDataTypes.MediaPicker, description: "If this content requires partner logos, add them here. They should be small.", sortOrder: 106)] 
        public string PhDefLogo03 { get; set; }

        [UmbracoProperty("Logo 4", "phDefLogo04", BuiltInUmbracoDataTypes.MediaPicker, description: "If this content requires partner logos, add them here. They should be small.", sortOrder: 107)] 
        public string PhDefLogo04 { get; set; }

        [UmbracoProperty("Logo 5", "phDefLogo05", BuiltInUmbracoDataTypes.MediaPicker, description: "If this content requires partner logos, add them here. They should be small.", sortOrder: 108)] 
        public string PhDefLogo05 { get; set; }

        [UmbracoProperty("Logo 6", "phDefLogo06", BuiltInUmbracoDataTypes.MediaPicker, description: "If this content requires partner logos, add them here. They should be small.", sortOrder: 109)] 
        public string PhDefLogo06 { get; set; }

        [UmbracoProperty("Logo 7", "phDefLogo07", BuiltInUmbracoDataTypes.MediaPicker, description: "If this content requires partner logos, add them here. They should be small.", sortOrder: 110)] 
        public string PhDefLogo07 { get; set; }

        [UmbracoProperty("Logo 8", "phDefLogo08", BuiltInUmbracoDataTypes.MediaPicker, description: "If this content requires partner logos, add them here. They should be small.", sortOrder: 111)] 
        public string PhDefLogo08 { get; set; }

        [UmbracoProperty("Related pages", "phDefRelatedPages", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 112)]
        public string PhDefRelatedPages { get; set; }

        [UmbracoProperty("Related websites", "phDefRelatedSites", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 113)]
        public string PhDefRelatedSites { get; set; }
    }
}