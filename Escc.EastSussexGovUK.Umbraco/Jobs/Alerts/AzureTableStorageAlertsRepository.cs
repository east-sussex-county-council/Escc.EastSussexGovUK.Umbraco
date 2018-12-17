using System;
using System.Text;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Exceptionless;
using System.Configuration;
using Umbraco.Core.Logging;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Stores and retrieves job alerts data in Azure table storage
    /// </summary>
    /// <seealso cref="Escc.Jobs.Alerts.IJobAlertsRepository" />
    public class AzureTableStorageAlertsRepository : IJobAlertsRepository
    {
        private readonly IJobSearchQueryConverter _queryConverter;
        private readonly CloudTableClient _tableClient;
        private const string _alertsTable = "JobAlerts";
        private const string _alertsSentTable = "JobAlertsSent";

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableStorageAlertsRepository"/> class.
        /// </summary>
        /// <param name="queryConverter">The query converter.</param>
        /// <exception cref="ArgumentNullException">queryConverter</exception>
        public AzureTableStorageAlertsRepository(IJobSearchQueryConverter queryConverter, string azureStorageConnectionString)
        {
            if (queryConverter == null) throw new ArgumentNullException(nameof(queryConverter));
            if (string.IsNullOrEmpty(azureStorageConnectionString))
            {
                throw new ArgumentException("azureStorageConnectionString was not specified", nameof(azureStorageConnectionString));
            }

            _queryConverter = queryConverter;

            var storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
            _tableClient = storageAccount.CreateCloudTableClient();
        }

        /// <summary>
        /// Gets all alerts matching the supplied search query
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IEnumerable<JobAlert> GetAlerts(JobAlertsQuery query)
        {
            var table = _tableClient.GetTableReference(_alertsTable);
            table.CreateIfNotExistsAsync().Wait();

            var filter = TableQuery.GenerateFilterCondition("JobsSet", QueryComparisons.Equal, query.JobsSet.ToString());

            if (!String.IsNullOrEmpty(query.EmailAddress))
            {
                filter = TableQuery.CombineFilters(filter, TableOperators.And, TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, query.EmailAddress));
            }

            if (query.Frequency.HasValue)
            {
                filter = TableQuery.CombineFilters(filter, TableOperators.And, TableQuery.GenerateFilterConditionForInt("Frequency", QueryComparisons.Equal, query.Frequency.Value));
            }

            var tableQuery = new TableQuery<JobAlertTableEntity>().Where(filter);
            var results = ReadAllResults(table, tableQuery, entity => BuildAlertFromEntity(entity));

            return results;
        }


        /// <summary>
        /// Gets a single alert by its identifier.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <returns></returns>
        public JobAlert GetAlertById(string alertId)
        {
            JobAlertTableEntity entity = ReadAlertEntity(alertId);
            return BuildAlertFromEntity(entity);
        }

        private JobAlert BuildAlertFromEntity(JobAlertTableEntity entity)
        {
            JobAlert alert = null;
            if (entity != null)
            {
                var searchQuery = HttpUtility.ParseQueryString(String.IsNullOrEmpty(entity.Criteria) ? String.Empty : entity.Criteria);
                alert = new JobAlert()
                {
                    AlertId = entity.RowKey,
                    Query = _queryConverter.ToQuery(searchQuery),
                    Email = entity.PartitionKey,
                    Frequency = entity.Frequency,
                    JobsSet = (JobsSet)Enum.Parse(typeof(JobsSet), entity.JobsSet)
                };
                alert.Query.JobsSet = alert.JobsSet;
            }

            return alert;
        }

        private JobAlertTableEntity ReadAlertEntity(string alertId)
        {
            var table = _tableClient.GetTableReference(_alertsTable);

            TableQuery<JobAlertTableEntity> tableQuery = new TableQuery<JobAlertTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, alertId));

            return table.ExecuteQuery(tableQuery).FirstOrDefault();
        }

        /// <summary>
        /// Saves a new or updated alert.
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <exception cref="ArgumentNullException">alert</exception>
        /// <exception cref="ArgumentException">The alert must have an AlertId - alert</exception>
        public void SaveAlert(JobAlert alert)
        {
            if (alert == null) throw new ArgumentNullException(nameof(alert));
            if (String.IsNullOrEmpty(alert.AlertId)) throw new ArgumentException("The alert must have an AlertId", nameof(alert));

            var table = _tableClient.GetTableReference(_alertsTable);
            table.CreateIfNotExistsAsync().Wait();

            var query = _queryConverter.ToCollection(alert.Query);
            query.Remove("page");
            query.Remove("pagesize");
            query.Remove("sort");
            var serialised = query.ToString();

            // Azure tables use an index clustered first by partition key then by row key,
            // so use email as the partition key to make it easy to get all alerts for a user.
            var entity = new JobAlertTableEntity()
            {
                PartitionKey = ToAzureKeyString(alert.Email),
                RowKey = alert.AlertId,
                Criteria = serialised,
                Frequency = alert.Frequency,
                JobsSet = alert.JobsSet.ToString()
            };

            try
            {
                table.Execute(TableOperation.InsertOrReplace(entity));
            }
            catch (StorageException ex)
            {
                if (ex.Message.Contains("(400) Bad Request"))
                {
                    var alertQuery = _queryConverter.ToCollection(alert.Query).ToString();
                    LogHelper.Error<AzureTableStorageAlertsRepository>(alertQuery + " returned " + ex.RequestInformation.ExtendedErrorInformation.ErrorMessage, ex);
                    ex.ToExceptionless().Submit();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Cancels an alert. If it's the last alert for an email address, also removes all data for that email address within that <see cref="JobsSet"/>.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">alertId</exception>
        public bool CancelAlert(string alertId)
        {
            if (String.IsNullOrEmpty(alertId)) throw new ArgumentNullException(nameof(alertId));

            var alertsTable = _tableClient.GetTableReference(_alertsTable);
            alertsTable.CreateIfNotExistsAsync().Wait();

            var alertEntity = ReadAlertEntity(alertId);
            if (alertEntity == null) return false;

            var emailAddress = alertEntity.PartitionKey;
            var jobsSet = (JobsSet)Enum.Parse(typeof(JobsSet), alertEntity.JobsSet);

            try
            {
                alertsTable.Execute(TableOperation.Delete(alertEntity));
            }
            catch (StorageException ex)
            {
                if (ex.Message.Contains("(400) Bad Request"))
                {
                    LogHelper.Error<AzureTableStorageAlertsRepository>($"Delete job alert {alertId} returned {ex.RequestInformation.ExtendedErrorInformation.ErrorMessage}", ex);
                    ex.ToExceptionless().Submit();
                }
                else
                {
                    throw;
                }
            }

            DeleteRecordOfAlertsSent(jobsSet, emailAddress);

            return true;
        }

        private void DeleteRecordOfAlertsSent(JobsSet jobsSet, string emailAddress)
        {
            // Only remove data if there are no more alerts set up for this email, otherwise we may still send jobs they've already seen
            var alertsForThisEmail = GetAlerts(new JobAlertsQuery { JobsSet = jobsSet, EmailAddress = emailAddress });
            if (alertsForThisEmail.Any()) return;

            // Delete the record of alerts sent
            var table = _tableClient.GetTableReference(_alertsSentTable);
            table.CreateIfNotExistsAsync().Wait();

            var tableQuery = new TableQuery<TableEntity>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, emailAddress),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("JobsSet", QueryComparisons.Equal, jobsSet.ToString())));

            var alertsSentEntities = table.ExecuteQuery(tableQuery);
            foreach (var entity in alertsSentEntities)
            {
                try
                {
                    table.Execute(TableOperation.Delete(entity));
                }
                catch (StorageException ex)
                {
                    if (ex.Message.Contains("(400) Bad Request"))
                    {
                        LogHelper.Error<AzureTableStorageAlertsRepository>($"Delete job alert sent record for job {entity.RowKey} and alert {emailAddress} returned {ex.RequestInformation.ExtendedErrorInformation.ErrorMessage}", ex);
                        ex.ToExceptionless().Submit();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
        }

        /// <summary>
        /// Removes characters from a string that would not be valid for use as a partition key or row key
        /// </summary>
        /// <param name="str">The string.</param>
        /// <remarks>From http://stackoverflow.com/questions/14859405/azure-table-storage-returns-400-bad-request</remarks>
        /// <returns></returns>
        public static string ToAzureKeyString(string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str
                .Where(c => c != '/'
                            && c != '\\'
                            && c != '#'
                            && c != '/'
                            && c != '?'
                            && !char.IsControl(c)))
                sb.Append(c);
            return sb.ToString();
        }

        /// <summary>
        /// Records that an alert has been sent to a given email address.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        public void MarkAlertAsSent(JobsSet jobsSet, int jobId, string emailAddress)
        {
            var table = _tableClient.GetTableReference(_alertsSentTable);
            table.CreateIfNotExistsAsync().Wait();

            var entity = new JobAlertSentTableEntity()
            {
                PartitionKey = ToAzureKeyString(emailAddress),
                RowKey = jobId.ToString(),
                JobsSet = jobsSet.ToString()
            };

            try
            {
                var result = table.Execute(TableOperation.InsertOrReplace(entity));
            }
            catch (StorageException ex)
            {
                if (ex.Message.Contains("(400) Bad Request"))
                {
                    LogHelper.Error<AzureTableStorageAlertsRepository>($"Mark job alert for job {jobId} as sent to {emailAddress} returned {ex.RequestInformation.ExtendedErrorInformation.ErrorMessage}", ex);
                    ex.ToExceptionless().Submit();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the ids of the jobs already sent to a given email address.
        /// </summary>
        /// <param name="jobsSet">The jobs set.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public IList<int> GetJobsSentForEmail(JobsSet jobsSet, string emailAddress)
        {
            // Create the table query.
            var table = _tableClient.GetTableReference(_alertsSentTable);

            // Initialize a default TableQuery to retrieve all the entities in the table.
            var tableQuery = new TableQuery<TableEntity>().Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, emailAddress),
                    TableOperators.And, 
                    TableQuery.GenerateFilterCondition("JobsSet", QueryComparisons.Equal, jobsSet.ToString()))
                    );

            return ReadAllResults<int>(table, tableQuery, entity => Int32.Parse(entity.RowKey));
        }

        private static IList<ItemType> ReadAllResults<ItemType>(CloudTable table, TableQuery<TableEntity> tableQuery, Func<TableEntity, ItemType> buildItem)
        {
            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;

            var results = new List<ItemType>();

            do
            {
                // Retrieve a segment (up to 1,000 entities).
                var tableQueryResult = Task.Run(() => table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken)).Result;

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;

                foreach (var entity in tableQueryResult.Results)
                {
                    results.Add(buildItem(entity));
                }

                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);

            return results;
        }

        private static IList<ItemType> ReadAllResults<ItemType>(CloudTable table, TableQuery<JobAlertTableEntity> tableQuery, Func<JobAlertTableEntity, ItemType> buildItem)
        {
            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;

            var results = new List<ItemType>();

            do
            {
                // Retrieve a segment (up to 1,000 entities).
                var tableQueryResult = Task.Run(() => table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken)).Result;

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;

                foreach (var entity in tableQueryResult.Results)
                {
                    results.Add(buildItem(entity));
                }

                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);

            return results;
        }
    }
}