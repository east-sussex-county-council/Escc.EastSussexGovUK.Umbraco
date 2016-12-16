using System.Collections.Generic;
using System.Web;
using Escc.AddressAndPersonalDetails;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.Models
{
    public class PersonViewModel : BaseViewModel
    {
        public PersonViewModel()
        {
            Metadata.PersonAbout = new Person();
            RelatedLinksGroups = new List<LandingSectionViewModel>();
        }

        public string JobTitle { get; set; }

        public IHtmlString LeadingText { get; set; }

        public string Subheading1 { get; set; }
        public string Subheading2 { get; set; }

        public IHtmlString Content1 { get; set; }
        public IHtmlString Content2 { get; set; }
        public IHtmlString Contact { get; set; }

        public Image Photo { get; set; }

        public IList<LandingSectionViewModel> RelatedLinksGroups { get; private set; }
    }
}