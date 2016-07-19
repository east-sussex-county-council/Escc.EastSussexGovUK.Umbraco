using System.Web;
using Escc.EastSussexGovUK.MasterPages.Features;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    public class BaseViewModelWithInheritedContent : BaseViewModel
    {
        public IHtmlString Latest { get; set; }
        public bool ShowEastSussex1SpaceWidget { get; set; }
        public bool ShowEscisWidget { get; set; }
        public SocialMediaSettings SocialMedia { get; set; }
        public WebChatSettings WebChat { get; set; }
    }
}