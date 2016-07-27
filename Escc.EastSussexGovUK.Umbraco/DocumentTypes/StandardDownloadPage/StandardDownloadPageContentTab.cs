using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage
{
    public class StandardDownloadPageContentTab : TabBase
    {
        [UmbracoProperty("Image", "phDefImage01", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 1)]
        public string PhDefImage01 { get; set; }

        [UmbracoProperty("Caption for image", "phDefCaption01", BuiltInUmbracoDataTypes.Textbox, sortOrder: 2)] 
        public string PhDefCaption01 { get; set; }

        [UmbracoProperty("Use image's alt text as its caption", "phDefAltAsCaption01", CheckboxDataType.PropertyEditorAlias, CheckboxDataType.DataTypeName, sortOrder: 3)]
        public string PhDefAltAsCaption01 { get; set; }

        [UmbracoProperty("Introductory text", "phDefIntro", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 4)]
        public string PhDefIntro { get; set; }

        [UmbracoProperty("Document 1", "phDefFile01", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 5)]
        public string PhDefFile01 { get; set; }

        [UmbracoProperty("Document 1 (alternative format)", "phDefFile01a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 6)]
        public string PhDefFile01A { get; set; }

        [UmbracoProperty("Document 2", "phDefFile02", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 7)]
        public string PhDefFile02 { get; set; }

        [UmbracoProperty("Document 2 (alternative format)", "phDefFile02a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 8)]
        public string PhDefFile02A { get; set; }

        [UmbracoProperty("Document 3", "phDefFile03", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 9)]
        public string PhDefFile03 { get; set; }

        [UmbracoProperty("Document 3 (alternative format)", "phDefFile03a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 10)]
        public string PhDefFile03A { get; set; }

        [UmbracoProperty("Document 4", "phDefFile04", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 11)]
        public string PhDefFile04 { get; set; }

        [UmbracoProperty("Document 4 (alternative format)", "phDefFile04a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 12)]
        public string PhDefFile04A { get; set; }

        [UmbracoProperty("Document 5", "phDefFile05", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 13)]
        public string PhDefFile05 { get; set; }

        [UmbracoProperty("Document 5 (alternative format)", "phDefFile05a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 14)]
        public string PhDefFile05A { get; set; }

        [UmbracoProperty("Document 6", "phDefFile06", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 15)]
        public string PhDefFile06 { get; set; }

        [UmbracoProperty("Document 6 (alternative format)", "phDefFile06a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 16)]
        public string PhDefFile06A { get; set; }

        [UmbracoProperty("Document 7", "phDefFile07", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 17)]
        public string PhDefFile07 { get; set; }

        [UmbracoProperty("Document 7 (alternative format)", "phDefFile07a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 18)]
        public string PhDefFile07A { get; set; }

        [UmbracoProperty("Document 8", "phDefFile08", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 19)]
        public string PhDefFile08 { get; set; }

        [UmbracoProperty("Document 8 (alternative format)", "phDefFile08a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 20)]
        public string PhDefFile08A { get; set; }

        [UmbracoProperty("Document 9", "phDefFile09", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 21)]
        public string PhDefFile09 { get; set; }

        [UmbracoProperty("Document 9 (alternative format)", "phDefFile09a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 22)]
        public string PhDefFile09A { get; set; }

        [UmbracoProperty("Document 10", "phDefFile10", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 23)]
        public string PhDefFile10 { get; set; }

        [UmbracoProperty("Document 10 (alternative format)", "phDefFile10a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 24)]
        public string PhDefFile10A { get; set; }

        [UmbracoProperty("Document 11", "phDefFile11", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 25)]
        public string PhDefFile11 { get; set; }

        [UmbracoProperty("Document 11 (alternative format)", "phDefFile11a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 26)]
        public string PhDefFile11A { get; set; }

        [UmbracoProperty("Document 12", "phDefFile12", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 27)]
        public string PhDefFile12 { get; set; }

        [UmbracoProperty("Document 12 (alternative format)", "phDefFile12a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 28)]
        public string PhDefFile12A { get; set; }

        [UmbracoProperty("Document 13", "phDefFile13", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 29)]
        public string PhDefFile13 { get; set; }

        [UmbracoProperty("Document 13 (alternative format)", "phDefFile13a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 30)]
        public string PhDefFile13A { get; set; }

        [UmbracoProperty("Document 14", "phDefFile14", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 31)]
        public string PhDefFile14 { get; set; }

        [UmbracoProperty("Document 14 (alternative format)", "phDefFile14a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 32)]
        public string PhDefFile14A { get; set; }

        [UmbracoProperty("Document 15", "phDefFile15", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 33)]
        public string PhDefFile15 { get; set; }

        [UmbracoProperty("Document 15 (alternative format)", "phDefFile15a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 34)]
        public string PhDefFile15A { get; set; }

        [UmbracoProperty("Document 16", "phDefFile16", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 35)]
        public string PhDefFile16 { get; set; }

        [UmbracoProperty("Document 16 (alternative format)", "phDefFile16a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 36)]
        public string PhDefFile16A { get; set; }

        [UmbracoProperty("Document 17", "phDefFile17", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 37)]
        public string PhDefFile17 { get; set; }

        [UmbracoProperty("Document 17 (alternative format)", "phDefFile17a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 38)]
        public string PhDefFile17A { get; set; }

        [UmbracoProperty("Document 18", "phDefFile18", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 39)]
        public string PhDefFile18 { get; set; }

        [UmbracoProperty("Document 18 (alternative format)", "phDefFile18a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 40)]
        public string PhDefFile18A { get; set; }

        [UmbracoProperty("Document 19", "phDefFile19", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 41)]
        public string PhDefFile19 { get; set; }

        [UmbracoProperty("Document 19 (alternative format)", "phDefFile19a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 42)]
        public string PhDefFile19A { get; set; }

        [UmbracoProperty("Document 20", "phDefFile20", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 43)]
        public string PhDefFile20 { get; set; }

        [UmbracoProperty("Document 20 (alternative format)", "phDefFile20a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 44)]
        public string PhDefFile20A { get; set; }

        [UmbracoProperty("Document 21", "phDefFile21", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 45)]
        public string PhDefFile21 { get; set; }

        [UmbracoProperty("Document 21 (alternative format)", "phDefFile21a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 46)]
        public string PhDefFile21A { get; set; }

        [UmbracoProperty("Document 22", "phDefFile22", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 47)]
        public string PhDefFile22 { get; set; }

        [UmbracoProperty("Document 22 (alternative format)", "phDefFile22a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 48)]
        public string PhDefFile22A { get; set; }

        [UmbracoProperty("Document 23", "phDefFile23", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 49)]
        public string PhDefFile23 { get; set; }

        [UmbracoProperty("Document 23 (alternative format)", "phDefFile23a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 50)]
        public string PhDefFile23A { get; set; }

        [UmbracoProperty("Document 24", "phDefFile24", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 51)]
        public string PhDefFile24 { get; set; }

        [UmbracoProperty("Document 24 (alternative format)", "phDefFile24a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 52)]
        public string PhDefFile24A { get; set; }

        [UmbracoProperty("Document 25", "phDefFile25", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 53)]
        public string PhDefFile25 { get; set; }

        [UmbracoProperty("Document 25 (alternative format)", "phDefFile25a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 54)]
        public string PhDefFile25A { get; set; }

        [UmbracoProperty("Document 26", "phDefFile26", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 55)]
        public string PhDefFile26 { get; set; }

        [UmbracoProperty("Document 26 (alternative format)", "phDefFile26a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 56)]
        public string PhDefFile26A { get; set; }

        [UmbracoProperty("Document 27", "phDefFile27", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 57)]
        public string PhDefFile27 { get; set; }

        [UmbracoProperty("Document 27 (alternative format)", "phDefFile27a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 58)]
        public string PhDefFile27A { get; set; }

        [UmbracoProperty("Document 28", "phDefFile28", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 59)]
        public string PhDefFile28 { get; set; }

        [UmbracoProperty("Document 28 (alternative format)", "phDefFile28a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 60)]
        public string PhDefFile28A { get; set; }

        [UmbracoProperty("Document 29", "phDefFile29", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 61)]
        public string PhDefFile29 { get; set; }

        [UmbracoProperty("Document 29 (alternative format)", "phDefFile29a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 62)]
        public string PhDefFile29A { get; set; }

        [UmbracoProperty("Document 30", "phDefFile30", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 63)]
        public string PhDefFile30 { get; set; }

        [UmbracoProperty("Document 30 (alternative format)", "phDefFile30a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 64)]
        public string PhDefFile30A { get; set; }

        [UmbracoProperty("Document 31", "phDefFile31", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 65)]
        public string PhDefFile31 { get; set; }

        [UmbracoProperty("Document 31 (alternative format)", "phDefFile31a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 66)]
        public string PhDefFile31A { get; set; }

        [UmbracoProperty("Document 32", "phDefFile32", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 67)]
        public string PhDefFile32 { get; set; }

        [UmbracoProperty("Document 32 (alternative format)", "phDefFile32a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 68)]
        public string PhDefFile32A { get; set; }

        [UmbracoProperty("Document 33", "phDefFile33", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 69)]
        public string PhDefFile33 { get; set; }

        [UmbracoProperty("Document 33 (alternative format)", "phDefFile33a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 70)]
        public string PhDefFile33A { get; set; }

        [UmbracoProperty("Document 34", "phDefFile34", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 71)]
        public string PhDefFile34 { get; set; }

        [UmbracoProperty("Document 34 (alternative format)", "phDefFile34a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 72)]
        public string PhDefFile34A { get; set; }

        [UmbracoProperty("Document 35", "phDefFile35", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 73)]
        public string PhDefFile35 { get; set; }

        [UmbracoProperty("Document 35 (alternative format)", "phDefFile35a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 74)]
        public string PhDefFile35A { get; set; }

        [UmbracoProperty("Document 36", "phDefFile36", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 75)]
        public string PhDefFile36 { get; set; }

        [UmbracoProperty("Document 36 (alternative format)", "phDefFile36a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 76)]
        public string PhDefFile36A { get; set; }

        [UmbracoProperty("Document 37", "phDefFile37", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 77)]
        public string PhDefFile37 { get; set; }

        [UmbracoProperty("Document 37 (alternative format)", "phDefFile37a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 78)]
        public string PhDefFile37A { get; set; }

        [UmbracoProperty("Document 38", "phDefFile38", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 79)]
        public string PhDefFile38 { get; set; }

        [UmbracoProperty("Document 38 (alternative format)", "phDefFile38a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 80)]
        public string PhDefFile38A { get; set; }

        [UmbracoProperty("Document 39", "phDefFile39", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 81)]
        public string PhDefFile39 { get; set; }

        [UmbracoProperty("Document 39 (alternative format)", "phDefFile39a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 82)]
        public string PhDefFile39A { get; set; }

        [UmbracoProperty("Document 40", "phDefFile40", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 83)]
        public string PhDefFile40 { get; set; }

        [UmbracoProperty("Document 40 (alternative format)", "phDefFile40a", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 84)]
        public string PhDefFile40A { get; set; }

        [UmbracoProperty("Footnote", "phDefFootnote", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 85)]
        public string PhDefFootnote{ get; set; }

        [UmbracoProperty("Related pages", "phDefRelatedPages", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 112)]
        public string PhDefRelatedPages { get; set; }

        [UmbracoProperty("Related websites", "phDefRelatedSites", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextLinksListDataType.DataTypeName, sortOrder: 113)]
        public string PhDefRelatedSites { get; set; }

    }
}
