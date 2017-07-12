using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    public interface IAlertsRepository
    {
        IEnumerable<JobAlert> GetAllAlerts(JobAlertsQuery query);
        void SaveAlert(JobAlert alert);
        JobAlert GetAlertById(string alertId);
        bool CancelAlert(string alertId);
        void MarkAlertAsSent(JobsSet jobsSet, string emailAddress, int jobId);
        IList<int> GetJobsSentForEmail(JobsSet jobsSet, string emailAddress);
    }
}