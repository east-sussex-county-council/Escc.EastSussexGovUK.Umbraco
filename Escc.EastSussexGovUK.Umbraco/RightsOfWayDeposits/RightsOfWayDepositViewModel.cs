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

        public PersonName Owner { get; set; }

        public BS7666Address Address { get; set; }

        public LatitudeLongitude Coordinates { get; set; }

        public string OrdnanceSurveyGridReference { get; set; }

        public IList<string> Parishes { get; private set; } = new List<string>();

        public Uri PageUrl { get; set; }

        public IList<HtmlLink> DepositDocuments { get; private set; } = new List<HtmlLink>();

        public DateTime DateDeposited { get; set; }

        public DateTime DateExpires { get; set; }
    }
}