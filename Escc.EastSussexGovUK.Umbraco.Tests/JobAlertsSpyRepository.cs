using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    internal class JobAlertsSpyRepository : IJobAlertsRepository
    {
        private List<int> _jobsSent = new List<int>();

        public Task<bool> CancelAlert(string alertId)
        {
            throw new NotImplementedException();
        }

        public JobAlert GetAlertById(string alertId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JobAlert>> GetAlerts(JobAlertsQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<IList<int>> GetJobsSentForEmail(JobsSet jobsSet, string emailAddress)
        {
            throw new NotImplementedException();
        }

        public void MarkAlertAsSent(JobsSet jobsSet, int jobId, string emailAddress)
        {
            if (!_jobsSent.Contains(jobId))
            {
                _jobsSent.Add(jobId);
                UniqueJobCount++;
            }
        }

        public void SaveAlert(JobAlert alert)
        {
            throw new NotImplementedException();
        }

        public int UniqueJobCount { get; set; }
    }
}
