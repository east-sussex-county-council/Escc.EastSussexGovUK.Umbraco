using System;
using System.Collections.Generic;
using System.Web;
using Escc.AddressAndPersonalDetails;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    /// <summary>
    /// A model of a service location to be displayed using the Location view
    /// </summary>
    public class LocationViewModel : BaseViewModelWithInheritedContent
    {
        public LocationViewModel()
        {
            RelatedLinksGroups = new List<LandingSectionViewModel>();
            WasteTypesRecycled = new List<string>();
            WasteTypesAccepted = new List<string>();
        }

        public IList<OpeningTimes> OpeningHours { get; set; }
        public IHtmlString OpeningHoursDetails { get; set; }
        public IHtmlString Content { get; set; }
        public string Tab1Title { get; set; }
        public IHtmlString Tab1Content { get; set; }
        public string Tab2Title { get; set; }
        public IHtmlString Tab2Content { get; set; }
        public string Tab3Title { get; set; }
        public IHtmlString Tab3Content { get; set; }
        public Image Photo { get; set; }
        public AddressInfo Location { get; set; }
        public DateTime? OpenUntil { get; set; }
        public DateTime? NextOpen { get; set; }
        public string NextOpenRelativeTime { get; set; }
        public string Email1Label { get; set; }
        public string Email2Label { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1Label { get; set; }
        public string Phone2Label { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax1Label { get; set; }
        public string Fax2Label { get; set; }
        public string Fax1 { get; set; }
        public string Fax2 { get; set; }
        public IList<LandingSectionViewModel> RelatedLinksGroups { get; private set; }
        public IList<string> WasteTypesRecycled { get; private set; }
        public IList<string> WasteTypesAccepted { get; private set; }
        public string ResponsibleAuthority { get; set; }
    }
}