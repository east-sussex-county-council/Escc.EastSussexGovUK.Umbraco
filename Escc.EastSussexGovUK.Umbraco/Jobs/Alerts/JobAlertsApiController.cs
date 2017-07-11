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
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Examine
{
    public class JobAlertsApiController : UmbracoApiController
    {
        [HttpGet]
        public void SendAlerts([FromUri]int? frequency)
        {
            Uri publicJobAdvert = null, redeploymentJobAdvert = null, publicJobAlerts = null, redeploymentJobAlerts = null;

            var jobAdvertPages = Umbraco.TypedContentAtXPath("//JobAdvert");
            foreach (var jobAdvert in jobAdvertPages)
            {
                var index = umbraco.library.GetPreValueAsString(jobAdvert.GetPropertyValue<int>("PublicOrRedeployment_Content"));
                if (index == "Redeployment jobs")
                {
                    redeploymentJobAdvert = new Uri(jobAdvert.UrlAbsolute());
                }
                else publicJobAdvert = new Uri(jobAdvert.UrlAbsolute());
            }

            var jobAlertsPages = Umbraco.TypedContentAtXPath("//JobAlerts");
            foreach (var jobAlert in jobAlertsPages)
            {
                var index = umbraco.library.GetPreValueAsString(jobAlert.GetPropertyValue<int>("PublicOrRedeployment_Content"));
                if (index == "Redeployment jobs")
                {
                    redeploymentJobAlerts = new Uri(jobAlert.UrlAbsolute());
                }
                else publicJobAlerts = new Uri(jobAlert.UrlAbsolute());
            }

            if (publicJobAdvert != null && publicJobAlerts != null)
            {
                SendAlertsForJobSet(JobsSet.PublicJobs, frequency, publicJobAdvert, publicJobAlerts);
            }
            if (redeploymentJobAdvert != null && redeploymentJobAlerts != null)
            {
                SendAlertsForJobSet(JobsSet.RedeploymentJobs, frequency, redeploymentJobAdvert, redeploymentJobAlerts);
            }
        }

        private void SendAlertsForJobSet(JobsSet jobsSet, int? frequency, Uri jobAdvertUrl, Uri alertsPageUrl)
        {
            IAlertsRepository repo = new AzureTableStorageAlertsRepository();
            var alerts = repo.GetAllAlerts(new JobAlertsQuery() { Frequency = frequency, JobsSet = jobsSet });
            var alertsGroupedByEmail = GroupAlertsByEmail(alerts);

            foreach (var alertsForAnEmail in alertsGroupedByEmail.Values)
            {
                foreach (var alert in alertsForAnEmail)
                {
                    var jobsSentForThisEmail = repo.GetJobsSentForEmail(jobsSet, alert.Email);
                    LookupJobsForAlert(alert, jobsSentForThisEmail, jobAdvertUrl, jobsSet);
                }

                var email = BuildEmail(alertsForAnEmail, alertsPageUrl, new JobAlertIdEncoder());

                if (!String.IsNullOrEmpty(email))
                {
                    SendEmail(alertsForAnEmail[0].Email, email);
                }

                foreach (var alert in alertsForAnEmail)
                {
                    foreach (var job in alert.MatchingJobs)
                    {
                        repo.MarkAlertAsSent(jobsSet, alert.Email, job.Id);
                    }
                }
            }
        }

        private async Task<IEnumerable<Job>> Search(JobSearchQuery query, Uri jobAdvertUrl, JobsSet jobsSet)
        {
            var source = new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection[jobsSet + "Searcher"], 
                new QueryBuilder(new LuceneTokenisedQueryBuilder(), new KeywordsTokeniser(), new LuceneStopWordsRemover(), new WildcardSuffixFilter()), 
                new RelativeJobUrlGenerator(jobAdvertUrl));
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
            var message = new MailMessage();
            message.To.Add(emailAddress);
            message.Subject = "Is this a job you'll love doing?";
            message.Body = emailHtml;
            message.IsBodyHtml = true;

            var configuration = new ConfigurationServiceRegistry();
            var cache = new HttpContextCacheStrategy();
            var emailService = ServiceContainer.LoadService<IEmailSender>(configuration, cache);
            emailService.SendAsync(message);
        }

        private static string BuildEmail(IList<JobAlert> alertsForAnEmail, Uri alertUrl, JobAlertIdEncoder encoder)
        {
            var emailHtml = new StringBuilder();

            foreach (var alert in alertsForAnEmail)
            {
                if (alert.MatchingJobs.Count > 0)
                {
                    var query = String.IsNullOrEmpty(alert.Criteria) ? new JobSearchQuery() : new JobSearchQueryConverter().ToQuery(HttpUtility.ParseQueryString(alert.Criteria));

                    emailHtml.Append("<h2>").Append(query).Append("</h2><ul>");
                    foreach (var job in alert.MatchingJobs)
                    {
                        emailHtml.Append("<li><a href=\"").Append(job.Url).Append("\">").Append(job.JobTitle).Append("</a></li>");
                    }
                    emailHtml.Append("</ul>");

                    emailHtml.Append("<p><a href=\"").Append(encoder.AddIdToUrl(alertUrl, alert.AlertId)).Append("\">Change or cancel alert</a></p>");
                }
            }
            return emailHtml.ToString();
        }

        private async void LookupJobsForAlert(JobAlert alert, IList<int> jobsSentForThisAlert, Uri jobAdvertUrl, JobsSet jobsSet)
        {
            var query = String.IsNullOrEmpty(alert.Criteria) ? new NameValueCollection() : HttpUtility.ParseQueryString(alert.Criteria);
            var parsedQuery = new JobSearchQueryConverter().ToQuery(query);
            parsedQuery.ClosingDateFrom = DateTime.Today;
            var jobs = await Search(parsedQuery, jobAdvertUrl, jobsSet);

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
