using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing
{
    public class LandingContentTab : TabBase
    {
        [UmbracoProperty("Layout", "layout", BuiltInUmbracoDataTypes.RadioButtonList, "Landing page layout", sortOrder: 0)]
        public string Layout { get; set; }

        [UmbracoProperty("Links 1", "links1", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 1)]
        public string Links1 { get; set; }

        [UmbracoProperty("Links 2", "links2", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 2)]
        public string Links2 { get; set; }

        [UmbracoProperty("Links 3", "links3", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 3)]
        public string Links3 { get; set; }

        [UmbracoProperty("Links 4", "links4", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 4)]
        public string Links4 { get; set; }

        [UmbracoProperty("Links 5", "links5", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 5)]
        public string Links5 { get; set; }

        [UmbracoProperty("Links 6", "links6", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 6)]
        public string Links6 { get; set; }

        [UmbracoProperty("Links 7", "links7", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 7)]
        public string Links7 { get; set; }

        [UmbracoProperty("Links 8", "links8", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 8)]
        public string Links8 { get; set; }

        [UmbracoProperty("Links 9", "links9", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 9)]
        public string Links9 { get; set; }

        [UmbracoProperty("Links 10", "links10", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 10)]
        public string Links10 { get; set; }

        [UmbracoProperty("Links 11", "links11", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 11)]
        public string Links11 { get; set; }

        [UmbracoProperty("Links 12", "links12", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 12)]
        public string Links12 { get; set; }

        [UmbracoProperty("Links 13", "links13", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 13)]
        public string Links13 { get; set; }

        [UmbracoProperty("Links 14", "links14", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 14)]
        public string Links14 { get; set; }

        [UmbracoProperty("Links 15", "links15", BuiltInUmbracoDataTypes.RelatedLinks,
            description: "The first link becomes the heading. Any others are listed beneath.", sortOrder: 15)]
        public string Links15 { get; set; }
    }
}