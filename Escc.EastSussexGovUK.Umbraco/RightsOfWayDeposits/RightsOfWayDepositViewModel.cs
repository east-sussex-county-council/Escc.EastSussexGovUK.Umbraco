using Escc.AddressAndPersonalDetails;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Geo;
using Escc.Umbraco.PropertyTypes;
using System;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Details of a single rights of way Section 31 deposit to be displayed on a detail page for that deposit
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class RightsOfWayDepositViewModel : BaseViewModel
    {
        public string Reference { get; set; }

        public IList<PersonName> IndividualOwners { get; private set; } = new List<PersonName>();

        public IList<string> OrganisationalOwners { get; private set; } = new List<string>();

        public IList<AddressInfo> Addresses { get; set; } = new List<AddressInfo>();

        public string OrdnanceSurveyGridReference { get; set; }

        public IList<string> Parishes { get; private set; } = new List<string>();

        public string Description { get; set; }

        public Uri PageUrl { get; set; }

        public IList<HtmlLink> DepositDocuments { get; private set; } = new List<HtmlLink>();

        public DateTime DateDeposited { get; set; }

        public DateTime DateExpires { get; set; }
    }
}