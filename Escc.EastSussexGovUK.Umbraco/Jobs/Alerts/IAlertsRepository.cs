namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    internal interface IAlertsRepository
    {
        void SaveAlert(JobAlert alert);
        JobAlert GetAlertById(string alertId);
        bool CancelAlert(string alertId);
    }
}