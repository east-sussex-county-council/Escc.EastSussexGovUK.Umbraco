using System;
using System.Runtime.Serialization;

namespace Escc.EastSussexGovUK.Umbraco.Web.Models
{
    [DataContract]
    public class OpeningTimes
    {
        #region JSON serialisable properties

        [DataMember(Name = "name")]
        public string Day { get; set; }

        [DataMember(Name = "scheduled")]
        public bool Scheduled { get; set; }

        [DataMember(Name = "open")]
        public string OpensAt { get; set; }

        [DataMember(Name = "close")]
        public string ClosesAt { get; set; }

        [DataMember(Name = "open2")]
        public string OpensAgainAt { get; set; }

        [DataMember(Name = "close2")]
        public string ClosesAgainAt { get; set; }

        #endregion

        #region Structured data

        private DateTime? _opensAt;
        private DateTime? _closesAt;
        private DateTime? _opensAgainAt;
        private DateTime? _closesAgainAt;

        public DayOfWeek DayOfWeek
        {
            get
            {
                DayOfWeek openingDay;
                DayOfWeek.TryParse(Day, true, out openingDay);
                return openingDay;
            }
        }

        public DateTime? OpensAtTime
        {
            get
            {
                if (!_opensAt.HasValue) _opensAt = ParseTime(OpensAt);
                return _opensAt;
            }
        }

        public DateTime? ClosesAtTime
        {
            get
            {
                if (!_closesAt.HasValue) _closesAt = ParseTime(ClosesAt);
                return _closesAt;
            }
        }


        public DateTime? OpensAgainAtTime
        {
            get
            {
                if (!_opensAgainAt.HasValue) _opensAgainAt = ParseTime(OpensAgainAt);
                return _opensAgainAt;
            }
        }

        public DateTime? ClosesAgainAtTime
        {
            get
            {
                if (!_closesAgainAt.HasValue) _closesAgainAt = ParseTime(ClosesAgainAt);
                return _closesAgainAt;
            }
        }

        #endregion

        private DateTime? ParseTime(string time)
        {
            if (String.IsNullOrEmpty(time)) return null;

            // We have a time and a day of the week, but for a DateTime object we need a real date. 
            // We want to have DateTime parse the time string, and to do that we need to append it to
            // a date string we can parse as well. .ToLongDateString() should be unambiguous.
            var date = DateTime.Parse(DateTime.Now.ToLongDateString() + " " + time);

            // Start from today and find out whether the day is earlier or later in the week, and adjust accordingly.
            var todayIndex = (int)DateTime.Now.DayOfWeek;
            var openingIndex = (int)DayOfWeek;
            var difference = openingIndex - todayIndex;
            date = date.AddDays(difference);

            // Because we're talking about opening times, earlier this week is no good. The same time next week is more useful.
            if (date.Date < DateTime.Now.Date) date = date.AddDays(7);
            return date;
        }
    }
}