using System;
using System.Collections.Generic;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// The content to be displayed on the website home page
    /// </summary>
    public class HomePageViewModel : BaseViewModelWithInheritedContent
    {
        public HomePageViewModel()
        {
            TopTasksLinks = new List<HtmlLink>();
            ReportLinks = new List<HtmlLink>();
            ApplyLinks = new List<HtmlLink>();
            PayLinks = new List<HtmlLink>();
            SchoolLinks = new List<HtmlLink>();
            LibrariesContent = new List<HtmlLink>();
            InvolvedItems = new List<HomePageItemViewModel>();
            InvolvedLinks = new List<HtmlLink>();
            NewsItems = new List<HomePageItemViewModel>();
        }

        public string TopTasksTitle { get; set; }
        public IList<HtmlLink> TopTasksLinks { get; private set; }

        public IList<HtmlLink> ReportLinks { get; private set; }
        public IList<HtmlLink> ApplyLinks { get; private set; }
        public IList<HtmlLink> PayLinks { get; private set; }

        public string NewsTitle { get; set; }
        public IList<HomePageItemViewModel> NewsItems { get; set; }
        public Uri NewsRssUrl { get; set; }

        public Uri TermDatesDataUrl { get; set; }
        public IList<HtmlLink> SchoolLinks { get; private set; }

        public string LibrariesTitle { get; set; }
        public IList<HtmlLink> LibrariesContent { get; private set; }
        public string RecyclingTitle { get; set; }

        public string InvolvedTitle { get; set; }
        public IList<HomePageItemViewModel> InvolvedItems { get; set; } 
        public IList<HtmlLink> InvolvedLinks { get; private set; }
        public Uri InvolvedRssUrl { get; set; }

    }
}