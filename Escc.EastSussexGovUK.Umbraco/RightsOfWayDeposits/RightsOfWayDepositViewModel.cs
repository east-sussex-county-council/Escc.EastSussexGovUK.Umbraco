using Escc.AddressAndPersonalDetails;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Geo;
using System;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    public class RightsOfWayDepositViewModel : BaseViewModel
    {
        public string Reference { get; set; }

        public PersonName Owner { get; set; }

        public BS7666Address Address { get; set; }

        public LatitudeLongitude Coordinates { get; set; }

        public string OrdnanceSurveyGridReference { get; set; }

        public string Parish { get; set; }

        public Uri DepositUrl { get; set; }

        private DateTime _dateDeposited;

        public DateTime DateDeposited
        {
            get
            {
                return _dateDeposited;
            }
            set
            {
                _dateDeposited = value;
                if (_dateDeposited >= new DateTime(2013,10,1))
                {
                    DateExpires = _dateDeposited.AddYears(20);
                }
                else
                {
                    DateExpires = _dateDeposited.AddYears(10);
                }
            }
        }

        public DateTime DateExpires { get; internal set; }
    }
}