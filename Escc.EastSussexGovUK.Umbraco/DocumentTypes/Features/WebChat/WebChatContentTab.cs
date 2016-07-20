using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.WebChat
{
    /// <summary>
    /// Content tab for the 'Web Chat' document type in Umbraco
    /// </summary>
    public class WebChatContentTab : TabBase
    {
        [UmbracoProperty("Where to display it?", "whereToDisplayIt", "Umbraco.MultiNodeTreePicker", "Multi-node tree picker", description: "Select the target pages on which to display web chat. This setting cascades to child pages.", sortOrder: 1)]
        public string WhereToDisplayIt { get; set; }

        [UmbracoProperty("Where else to display it?", "whereElseToDisplayIt", BuiltInUmbracoDataTypes.TextboxMultiple, description: "Paste any target page URLs, one per line. This is useful for targeting external applications like the online library catalogue.", sortOrder: 2)]
        public string WhereElseToDisplayIt { get; set; }

        [UmbracoProperty("Where to exclude?", "whereToExclude", "Umbraco.MultiNodeTreePicker", "Multi-node tree picker", description: "Select the target pages on which to hide web chat. This overrides the settings above.", sortOrder: 3)]
        public string WhereToExclude { get; set; }

        [UmbracoProperty("Where else to exclude?", "whereElseToExclude", BuiltInUmbracoDataTypes.TextboxMultiple, description: "Paste any target page URLs, one per line. This is useful for targeting external applications like the online library catalogue.", sortOrder: 4)]
        public string WhereElseToExclude { get; set; }
    }
}
