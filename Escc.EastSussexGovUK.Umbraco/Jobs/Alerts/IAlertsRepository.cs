﻿using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    public interface IAlertsRepository
    {
        IEnumerable<JobAlert> GetAllAlerts(string emailAddress = null);
        void SaveAlert(JobAlert alert);
        JobAlert GetAlertById(string alertId);
        bool CancelAlert(string alertId);
        void MarkAlertAsSent(string emailAddress, int jobId);
        IList<int> GetJobsSentForEmail(string emailAddress);
    }
}