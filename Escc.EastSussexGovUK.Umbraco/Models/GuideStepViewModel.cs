using System;
using System.Collections.Generic;
using System.Web;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    public class GuideStepViewModel : BaseViewModelWithInheritedContent
    {
        public GuideStepViewModel()
        {
            RelatedLinksGroups = new List<LandingSectionViewModel>();
            PartnerImages = new List<Image>();
        }

        public Uri GuideUrl { get; set; }
        public string GuideTitle { get; set; }
        public IList<GuideNavigationLink> Steps { get; set; }
        public IHtmlString StepContent { get; set; }
        public IList<LandingSectionViewModel> RelatedLinksGroups { get; private set; }
        public IList<Image> PartnerImages { get; private set; }
    }
}