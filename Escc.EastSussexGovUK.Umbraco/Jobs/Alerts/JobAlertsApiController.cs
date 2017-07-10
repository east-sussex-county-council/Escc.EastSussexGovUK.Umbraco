using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Examine;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
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
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Examine
{
    public class JobAlertsApiController : UmbracoApiController
    {
        [HttpGet]
        public void SendAlerts([FromUri]int? frequency)
        {
            IAlertsRepository repo = new AzureTableStorageAlertsRepository();
            var alerts = repo.GetAllAlerts(new JobAlertsQuery() { Frequency = frequency });
            var alertsGroupedByEmail = GroupAlertsByEmail(alerts);

            foreach (var alertsForAnEmail in alertsGroupedByEmail.Values)
            {
                foreach (var alert in alertsForAnEmail)
                {
                    var jobsSentForThisEmail = repo.GetJobsSentForEmail(alert.Email);
                    LookupJobsForAlert(alert, jobsSentForThisEmail);
                }

                var email = BuildEmail(alertsForAnEmail, new Uri(Request.RequestUri, "/jobs/alerts/"), new JobAlertIdEncoder());

                if (!String.IsNullOrEmpty(email))
                {
                    SendEmail(alertsForAnEmail[0].Email, email);
                }

                foreach (var alert in alertsForAnEmail)
                {
                    foreach (var job in alert.MatchingJobs)
                    {
                        repo.MarkAlertAsSent(alert.Email, job.Id);
                    }
                }
            }
        }

        private async Task<IEnumerable<Job>> Search(JobSearchQuery query)
        {
            var source = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection["PublicJobsSearcher"], 
                new QueryBuilder(new LuceneTokenisedQueryBuilder(), new KeywordsTokeniser(), new LuceneStopWordsRemover(), new WildcardSuffixFilter()), 
                new RelativeJobUrlGenerator(new Uri(Request.RequestUri,"/jobs/job/")));
            var jobs = await source.ReadJobs(query);
            return jobs;
        }

        private Dictionary<string, IList<JobAlert>> GroupAlertsByEmail(IEnumerable<JobAlert> subscriptions)
        {
            var subscriptionsGroupedByEmail = new Dictionary<string, IList<JobAlert>>();
            foreach (var subscription in subscriptions)
            {
                var email = subscription.Email.ToLowerInvariant();
                if (!subscriptionsGroupedByEmail.ContainsKey(email))
                {
                    subscriptionsGroupedByEmail.Add(email, new List<JobAlert>());
                }
                subscriptionsGroupedByEmail[email].Add(subscription);
            }
            return subscriptionsGroupedByEmail;
        }


        private static void SendEmail(string emailAddress, string emailHtml)
        {
            using (var smtp = new SmtpClient())
            {
                var message = new MailMessage();
                message.To.Add(emailAddress);
                message.Subject = "Is this a job you'll love doing?";
                message.Body = emailHtml;
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }

        private static string BuildEmail(IList<JobAlert> subscriptionsForAnEmail, Uri subscriptionUrl, JobAlertIdEncoder encoder)
        {
            var emailHtml = new StringBuilder();

            foreach (var subscription in subscriptionsForAnEmail)
            {
                if (subscription.MatchingJobs.Count > 0)
                {
                    emailHtml.Append("<h2>").Append(subscription.Criteria).Append("</h2><ul>");
                    foreach (var job in subscription.MatchingJobs)
                    {
                        emailHtml.Append("<li><a href=\"").Append(job.Url).Append("\">").Append(job.JobTitle).Append("</a></li>");
                    }
                    emailHtml.Append("</ul>");

                    emailHtml.Append("<p><a href=\"").Append(encoder.AddIdToUrl(subscriptionUrl, subscription.AlertId)).Append("\">Change or cancel alert</a></p>");
                }
            }
            return emailHtml.ToString();
        }

        private async void LookupJobsForAlert(JobAlert alert, IList<int> jobsSentForThisAlert)
        {
            var query = String.IsNullOrEmpty(alert.Criteria) ? new NameValueCollection() : HttpUtility.ParseQueryString(alert.Criteria);
            var parsedQuery = new JobSearchQueryConverter().ToQuery(query);
            parsedQuery.ClosingDateFrom = DateTime.Today;
            var jobs = await Search(parsedQuery);

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
