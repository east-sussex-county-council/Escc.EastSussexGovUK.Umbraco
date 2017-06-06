using Escc.AddressAndPersonalDetails;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Geo;
using System;

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

        public string Parish { get; set; }

        public Uri PageUrl { get; set; }

        public Uri DepositUrl { get; set; }

        public DateTime DateDeposited { get; set; }

        public DateTime DateExpires { get; set; }
    }
}