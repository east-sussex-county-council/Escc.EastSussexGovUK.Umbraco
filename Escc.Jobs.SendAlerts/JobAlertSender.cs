using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using Escc.EastSussexGovUK.Umbraco.Jobs.Api;
using Escc.EastSussexGovUK.Umbraco.WebApi;
using Escc.Net;
using Escc.Services;
using log4net;
using Newtonsoft.Json;

namespace Escc.Jobs.SendAlerts
{
    /// <summary>
    /// Work out which job alerts to send and send them
    /// </summary>
    public class JobAlertSender
    {
        private readonly ILog _log;

        /// <summary>
        /// Creates a new <see cref="JobAlertSender"/>
        /// </summary>
        /// <param name="log"></param>
        public JobAlertSender(ILog log) { _log = log; }

        /// <summary>
        /// Sends job alerts which are configured for a particular frequency (in days), or any frequency if blank
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <param name="forceResend">If set to <c>true</c> force resend of alerts already sent (for testing)</param>
        public async Task SendAlerts(int? frequency, bool forceResend)
        {
            var apiBaseUrl = new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]);
            var cacheStrategy = new MemoryJobCacheStrategy(MemoryCache.Default);

            var publicAlertsSettings = JobAlertSettings(apiBaseUrl, JobsSet.PublicJobs);
            if (publicAlertsSettings != null)
            {
                var publicJobsProvider = new JobsDataFromApi(apiBaseUrl, JobsSet.PublicJobs, publicAlertsSettings.JobAdvertBaseUrl, new HttpClientProvider(), cacheStrategy);
                await SendAlertsForJobSet(publicJobsProvider, JobsSet.PublicJobs, frequency, publicAlertsSettings, forceResend);
            }

            var redeploymentAlertsSettings = JobAlertSettings(apiBaseUrl, JobsSet.RedeploymentJobs);
            if (redeploymentAlertsSettings != null)
            {
                var redeploymentJobsProvider = new JobsDataFromApi(apiBaseUrl, JobsSet.RedeploymentJobs, redeploymentAlertsSettings.JobAdvertBaseUrl, new HttpClientProvider(), cacheStrategy);
                await SendAlertsForJobSet(redeploymentJobsProvider, JobsSet.RedeploymentJobs, frequency, redeploymentAlertsSettings, forceResend);
            }
        }


        /// <summary>
        /// Gets the job alert settings for a <see cref="JobsSet" />
        /// </summary>
        /// <returns></returns>
        public JobAlertSettings JobAlertSettings(Uri apiBaseUrl, JobsSet jobsSet)
        {
            var request = WebRequest.Create(new Uri($"{apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/{jobsSet}/jobalertsettings/"));
            _log.Info($"Requesting job alert settings from {request.RequestUri}");
            using (var response = request.GetResponse())
            {
                using (var responseReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseJson = responseReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<JobAlertSettings>(responseJson, new[] { new IHtmlStringConverter() });
                }
            }
        }

        private async Task SendAlertsForJobSet(IJobsDataProvider jobsProvider, JobsSet jobsSet, int? frequency, JobAlertSettings alertSettings, bool forceResend)
        {
            // No point sending alerts without links to the jobs
            if (alertSettings.JobAdvertBaseUrl == null)
            {
                _log.Error("JobAdvertBaseUrl not found - aborting");
                return;
            }

            // We need somewhere to get the alerts from...
            var converter = new JobSearchQueryConverter(ConfigurationManager.AppSettings["TranslateObsoleteJobTypes"]?.ToUpperInvariant() == "TRUE");
            var encoder = new JobAlertIdEncoder(converter);
            IJobAlertsRepository alertsRepo = new AzureTableStorageAlertsRepository(converter, ConfigurationManager.ConnectionStrings["JobAlerts.AzureStorage"].ConnectionString);

            // We need a way to send the alerts...
            var configuration = new ConfigurationServiceRegistry();
            var emailService = ServiceContainer.LoadService<IEmailSender>(configuration);
            var sender = new JobAlertsByEmailSender(alertSettings, new HtmlJobAlertFormatter(alertSettings, encoder), emailService);

            // Get them, sort them and send them
            _log.Info($"Requesting jobs matching {jobsSet} with frequency {frequency} from Azure Storage");
            var alerts = await alertsRepo.GetAlerts(new JobAlertsQuery() { Frequency = frequency, JobsSet = jobsSet });
            var alertsGroupedByEmail = GroupAlertsByEmail(alerts);

            _log.Info($"{alerts.Count()} alerts found for {alertsGroupedByEmail.Count()} email addresses");
            foreach (var alertsForAnEmail in alertsGroupedByEmail)
            {
                foreach (var alert in alertsForAnEmail)
                {
                    var jobsSentForThisEmail = forceResend ? new List<int>() : await alertsRepo.GetJobsSentForEmail(alert.JobsSet, alert.Email);
                    await LookupJobsForAlert(jobsProvider, alert, jobsSentForThisEmail);
                }
            }

            _log.Info("Sending alerts");
            await sender.SendGroupedAlerts(alertsGroupedByEmail, alertsRepo);
        }

        private IEnumerable<IList<JobAlert>> GroupAlertsByEmail(IEnumerable<JobAlert> alerts)
        {
            var groupedByEmail = new Dictionary<string, IList<JobAlert>>();
            foreach (var alert in alerts)
            {
                var email = alert.Email.ToLowerInvariant();
                if (!groupedByEmail.ContainsKey(email))
                {
                    groupedByEmail.Add(email, new List<JobAlert>());
                }
                groupedByEmail[email].Add(alert);
            }
            return groupedByEmail.Values;
        }

        private async Task LookupJobsForAlert(IJobsDataProvider jobsData, JobAlert alert, IList<int> jobsSentForThisAlert)
        {
            alert.Query.ClosingDateFrom = DateTime.Today;
            _log.Info($"Requesting {alert.Query.ToString()}");
            var jobs = await jobsData.ReadJobs(alert.Query);

            foreach (var job in jobs.Jobs)
            {
                if (!jobsSentForThisAlert.Contains(job.Id))
                {
                    alert.MatchingJobs.Add(job);
                }
            }
        }
    }
}
