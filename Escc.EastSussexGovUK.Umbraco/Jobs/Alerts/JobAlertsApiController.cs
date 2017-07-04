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
        public void SendAlerts()
        {
            var subscriptions = Subscriptions();
            var subscriptionsGroupedByEmail = GroupSubscriptionsByEmail(subscriptions);

            foreach (var subscriptionsForAnEmail in subscriptionsGroupedByEmail.Values)
            {
                LookupJobsForOneSubscriber(subscriptionsForAnEmail);

                var email = BuildEmail(subscriptionsForAnEmail, new Uri(Request.RequestUri, "/jobs/alerts/"), new JobAlertIdEncoder());

                SendEmail(subscriptionsForAnEmail[0].Email, email);
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

        private IEnumerable<JobAlert> Subscriptions()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString;
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new Exception("connectionStrings:Escc.EastSussexGovUK.Umbraco.AzureStorage not found in web.config");
            }


            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table query.
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("jobalerts");

            // Initialize a default TableQuery to retrieve all the entities in the table.
            TableQuery<JobAlertTableEntity> tableQuery = new TableQuery<JobAlertTableEntity>();
                //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "email.address@example.org"));

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;

            var alerts = new List<JobAlert>();

            do
            {
                // Retrieve a segment (up to 1,000 entities).
                TableQuerySegment<JobAlertTableEntity> tableQueryResult =
                    Task.Run(() => table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken)).Result;

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;

                // Print the number of rows retrieved.
                foreach (var entity in tableQueryResult.Results)
                {
                    alerts.Add(new JobAlert()
                    {
                        SubscriptionId = entity.RowKey,
                        Criteria = entity.Criteria,
                        Email = entity.PartitionKey,
                        Frequency = entity.Frequency
                    });
                }

                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);

            return alerts;
        }

        private Dictionary<string, IList<JobAlert>> GroupSubscriptionsByEmail(IEnumerable<JobAlert> subscriptions)
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
                emailHtml.Append("<h2>").Append(subscription.Criteria).Append("</h2><ul>");
                foreach (var job in subscription.MatchingJobs)
                {
                    emailHtml.Append("<li><a href=\"").Append(job.Url).Append("\">").Append(job.JobTitle).Append("</a></li>");
                }
                emailHtml.Append("</ul>");

                emailHtml.Append("<p><a href=\"").Append(encoder.AddIdToUrl(subscriptionUrl, subscription.SubscriptionId)).Append("\">Cancel subscription</a></p>");
            }

            return emailHtml.ToString();
        }

        private async void LookupJobsForOneSubscriber(IList<JobAlert> subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                var query = String.IsNullOrEmpty(subscription.Criteria) ? new NameValueCollection() : HttpUtility.ParseQueryString(subscription.Criteria);
                var parsedQuery = new JobSearchQueryFactory().CreateFromQueryString(query);
                parsedQuery.ClosingDateFrom = DateTime.Today;
                var jobs = await Search(parsedQuery);
                subscription.MatchingJobs.AddRange(jobs);
            }
        }
    }
}
