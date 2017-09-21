using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using Escc.Services;
using Examine;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Examine
{
    /// <summary>
    /// An API to trigger the sending of job alerts
    /// </summary>
    /// <seealso cref="Umbraco.Web.WebApi.UmbracoApiController" />
    [Authorize]
    public class JobAlertsApiController : UmbracoApiController
    {
        /// <summary>
        /// Sends job alerts which are configured for a particular frequency (in days), or any frequency if blank
        /// </summary>
        [HttpPost]
        public void SendAlerts()
        {
            SendAlerts(null, false);
        }

        /// <summary>
        /// Sends job alerts which are configured for a particular frequency (in days), or any frequency if blank
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        [HttpPost]
        public void SendAlerts([FromUri]int? frequency)
        {
            SendAlerts(frequency, false);
        }

        /// <summary>
        /// Sends job alerts which are configured for a particular frequency (in days), or any frequency if blank
        /// </summary>
        /// <param name="forceResend">if set to <c>true</c> force resend of alerts already sent (for testing).</param>
        [HttpPost]
        public void SendAlerts([FromUri]bool forceResend)
        {
            SendAlerts(null, forceResend);
        }

        /// <summary>
        /// Sends job alerts which are configured for a particular frequency (in days), or any frequency if blank
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        [HttpPost]
        public void SendAlerts([FromUri]int? frequency, [FromUri]bool forceResend)
        {
            var jobAlertsSettings = new JobAlertsSettingsFromUmbraco(Umbraco).GetJobAlertsSettings();

            if (jobAlertsSettings.ContainsKey(JobsSet.PublicJobs))
            {
                SendAlertsForJobSet(JobsSet.PublicJobs, frequency, jobAlertsSettings[JobsSet.PublicJobs], forceResend);
            }
            if (jobAlertsSettings.ContainsKey(JobsSet.RedeploymentJobs))
            {
                SendAlertsForJobSet(JobsSet.RedeploymentJobs, frequency, jobAlertsSettings[JobsSet.RedeploymentJobs], forceResend);
            }
        }

        private void SendAlertsForJobSet(JobsSet jobsSet, int? frequency, JobAlertSettings alertSettings, bool forceResend)
        {
            // No point sending alerts without links to the jobs
            if (alertSettings.JobAdvertBaseUrl == null) return;

            // We need somewhere to get the jobs from... 
            IJobsDataProvider jobsRepo = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "Searcher"],
                new QueryBuilder(new LuceneTokenisedQueryBuilder(), new KeywordsTokeniser(), new LuceneStopWordsRemover(), new WildcardSuffixFilter()), 
                new SalaryRangeLuceneQueryBuilder(),
                new RelativeJobUrlGenerator(alertSettings.JobAdvertBaseUrl));

            // We need somewhere to get the alerts from...
            var converter = new JobSearchQueryConverter();
            var encoder = new JobAlertIdEncoder(converter);
            IJobAlertsRepository alertsRepo = new AzureTableStorageAlertsRepository(converter);

            // We need a way to send the alerts...
            var configuration = new ConfigurationServiceRegistry();
            var cache = new HttpContextCacheStrategy();
            var emailService = ServiceContainer.LoadService<IEmailSender>(configuration, cache);
            var sender = new JobAlertsByEmailSender(alertSettings, new HtmlJobAlertFormatter(alertSettings, encoder), emailService);

            // Get them, sort them and send them
            var alerts = alertsRepo.GetAlerts(new JobAlertsQuery() { Frequency = frequency, JobsSet = jobsSet });
            var alertsGroupedByEmail = GroupAlertsByEmail(alerts);

            foreach (var alertsForAnEmail in alertsGroupedByEmail)
            {
                foreach (var alert in alertsForAnEmail)
                {
                    var jobsSentForThisEmail = forceResend ? new List<int>() : alertsRepo.GetJobsSentForEmail(alert.JobsSet, alert.Email);
                    LookupJobsForAlert(jobsRepo, alert, jobsSentForThisEmail);
                }
            }

            sender.SendGroupedAlerts(alertsGroupedByEmail, alertsRepo);
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

        private async void LookupJobsForAlert(IJobsDataProvider jobsData, JobAlert alert, IList<int> jobsSentForThisAlert)
        {
            alert.Query.ClosingDateFrom = DateTime.Today;
            var jobs = await jobsData.ReadJobs(alert.Query);

            foreach (var job in jobs)
            {
                if (!jobsSentForThisAlert.Contains(job.Id))
                {
                    alert.MatchingJobs.Add(job);
                }
            }
        }
    }
}
