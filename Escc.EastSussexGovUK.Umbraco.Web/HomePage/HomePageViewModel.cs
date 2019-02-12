using System;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.RubbishAndRecycling.SiteFinder.Website;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Web.HomePage
{
    /// <summary>
    /// The content to be displayed on the website home page
    /// </summary>
    public class HomePageViewModel : BaseViewModel
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
            RecyclingSiteSearch = new RecyclingViewModel();
        }

        public string TopTasksTitle { get; set; }
        public IList<HtmlLink> TopTasksLinks { get; private set; }

        public IList<HtmlLink> ReportLinks { get; private set; }
        public IList<HtmlLink> ApplyLinks { get; private set; }
        public IList<HtmlLink> PayLinks { get; private set; }

        public string NewsTitle { get; set; }
        public IList<HomePageItemViewModel> NewsItems { get; set; }
        public RecyclingViewModel RecyclingSiteSearch { get; }
        public Uri NewsRssUrl { get; set; }

        public Uri TermDatesDataUrl { get; set; }
        public IList<HtmlLink> SchoolLinks { get; private set; }

        public string LibrariesTitle { get; set; }
        public IList<HtmlLink> LibrariesContent { get; private set; }

        /// <summary>
        /// Gets or sets the logo for the jobs service
        /// </summary>
        public Image JobsLogo { get; set; }

        /// <summary>
        /// Gets or sets the jobs home page
        /// </summary>
        public HtmlLink JobsHomePage { get; set; }

        /// <summary>
        /// Gets or sets the job search results page
        /// </summary>
        public HtmlLink JobSearchResultsPage { get; set; }

        /// <summary>
        /// Gets or sets the job locations to list in the job search.
        /// </summary>
        public IList<JobsLookupValue> JobLocations{ get; set; } = new List<JobsLookupValue>();

        /// <summary>
        /// Gets or sets the job types to list in the job search.
        /// </summary>
        public IList<JobsLookupValue> JobTypes { get; set; } = new List<JobsLookupValue>();

        /// <summary>
        /// Gets or sets the title of the recycling search.
        /// </summary>
        public string RecyclingTitle { get; set; }

        /// <summary>
        /// Gets or sets the title of the 'Get involved' section.
        /// </summary>
        public string InvolvedTitle { get; set; }
        public IList<HomePageItemViewModel> InvolvedItems { get; set; } 
        public IList<HtmlLink> InvolvedLinks { get; private set; }
        public Uri InvolvedRssUrl { get; set; }

    }
}