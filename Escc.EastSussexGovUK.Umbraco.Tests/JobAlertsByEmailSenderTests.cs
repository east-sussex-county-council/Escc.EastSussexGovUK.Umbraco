using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class JobAlertsByEmailSenderTests
    {
        [Test]
        public void AllJobsMarkedAsSent()
        {
            var spy = new JobAlertsSpyRepository();
            var sender = new JobAlertsByEmailSender(new JobAlertSettings(), new FakeJobAlertFormatter(), new FakeEmailSender());
            var alerts = new List<List<JobAlert>>();
            alerts.Add(new List<JobAlert>()
            {
                new JobAlert() { AlertId = "123" }, new JobAlert() { AlertId = "456"}
            });
            alerts.Add(new List<JobAlert>()
            {
                new JobAlert() { AlertId = "abc" }, new JobAlert() { AlertId = "def"}
            });
            alerts[0][0].MatchingJobs.Add(new Job() { Id = 1 });
            alerts[0][0].MatchingJobs.Add(new Job() { Id = 2 });
            alerts[0][1].MatchingJobs.Add(new Job() { Id = 3 });
            alerts[0][1].MatchingJobs.Add(new Job() { Id = 4 });
            alerts[1][0].MatchingJobs.Add(new Job() { Id = 5 });
            alerts[1][0].MatchingJobs.Add(new Job() { Id = 6 });
            alerts[1][1].MatchingJobs.Add(new Job() { Id = 7 });
            alerts[1][1].MatchingJobs.Add(new Job() { Id = 8 });

            sender.SendGroupedAlerts(alerts, spy);

            Assert.AreEqual(8, spy.UniqueJobCount);
        }
    }
}
