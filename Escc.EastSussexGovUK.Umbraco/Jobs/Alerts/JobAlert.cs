using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    public class JobAlert
    {
        public string Criteria { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int Frequency { get; set; } = 1;

        public List<Job> MatchingJobs { get; private set; } = new List<Job>();
        public string SubscriptionId { get; set; }
    }
}