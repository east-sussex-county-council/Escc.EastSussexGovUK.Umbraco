using System;
using System.Collections.Generic;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Web.Landing;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Web.Task
{
    public class TaskViewModel : BaseViewModel
    {
        public TaskViewModel()
        {
            RelatedLinksGroups = new List<LandingSectionViewModel>();
            PartnerImages = new List<Image>();
        }

        public IHtmlString LeadingText { get; set; }
        public Uri StartPageUrl { get; set; }
        public string StartButtonText { get; set; }

        public string Subheading1 { get; set; }
        public string Subheading2 { get; set; }
        public string Subheading3 { get; set; }
        public string Subheading4 { get; set; }

        public IHtmlString Content1 { get; set; }
        public IHtmlString Content2 { get; set; }
        public IHtmlString Content3 { get; set; }
        public IHtmlString Content4 { get; set; }

        public IList<LandingSectionViewModel> RelatedLinksGroups { get; private set; }

        public IList<Image> PartnerImages { get; private set; }

    }
}