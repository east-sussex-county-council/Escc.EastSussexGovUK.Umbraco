﻿using System;
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
    public class AzureTableStorageAlertsRepository : IAlertsRepository
    {
        private readonly JobSearchQueryConverter _queryConverter;

        public AzureTableStorageAlertsRepository(JobSearchQueryConverter queryConverter)
        {
            if (queryConverter == null) throw new ArgumentNullException(nameof(queryConverter));
            _queryConverter = queryConverter;
        }

        public IEnumerable<JobAlert> GetAllAlerts(JobAlertsQuery query)
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
            var table = tableClient.GetTableReference("JobAlerts");

            // Initialize a default TableQuery to retrieve all the entities in the table.
            TableQuery<JobAlertTableEntity> tableQuery = new TableQuery<JobAlertTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("JobsSet", QueryComparisons.Equal, query.JobsSet.ToString()));

            if (!String.IsNullOrEmpty(query.EmailAddress))
            {
                tableQuery = tableQuery
                   .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, query.EmailAddress));
            }

            if (query.Frequency.HasValue)
            {
                tableQuery = tableQuery
                    .Where(TableQuery.GenerateFilterConditionForInt("Frequency", QueryComparisons.Equal, query.Frequency.Value));
            }

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
                    var searchQuery = HttpUtility.ParseQueryString(String.IsNullOrEmpty(entity.Criteria) ? String.Empty : entity.Criteria);
                    alerts.Add(new JobAlert()
                    {
                        AlertId = entity.RowKey,
                        Query = _queryConverter.ToQuery(searchQuery),
                        Email = entity.PartitionKey,
                        Frequency = entity.Frequency,
                        JobsSet = (JobsSet)Enum.Parse(typeof(JobsSet), entity.JobsSet)
                    });
                }

                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);

            return alerts;
        }

        public void SaveAlert(JobAlert alert)
        {
            if (alert == null) throw new ArgumentNullException(nameof(alert));
            if (String.IsNullOrEmpty(alert.AlertId)) throw new ArgumentException("The alert must have an AlertId", nameof(alert));

            // Get the storage account connection
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);

            // Create the table if it doesn't exist.
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("JobAlerts");
            table.CreateIfNotExistsAsync().Wait();

            // Azure tables use an index clustered first by partition key then by row key.
            var entity = new JobAlertTableEntity()
            {
                PartitionKey = ToAzureKeyString(alert.Email),
                RowKey = alert.AlertId,
                Criteria = _queryConverter.ToCollection(alert.Query).ToString(),
                Frequency = alert.Frequency,
                JobsSet = alert.JobsSet.ToString()
            };

            // Create the TableOperation object that inserts the entity.
            var insertOperation = TableOperation.InsertOrReplace(entity);

            // Execute the insert operation.
            try
            {
                table.Execute(insertOperation);
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

        public bool CancelAlert(string alertId)
        {
            // Get the storage account connection
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);

            // Create the table if it doesn't exist.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            var alertsTable = tableClient.GetTableReference("JobAlerts");
            alertsTable.CreateIfNotExistsAsync().Wait();

            var alertEntity = GetTableEntityByRowKey(alertId);
            if (alertEntity == null) return false;

            var emailAddress = alertEntity.PartitionKey;
            var jobsSet = (JobsSet)Enum.Parse(typeof(JobsSet), alertEntity.JobsSet);

            // Create the TableOperation object that deletes the entity.
            var deleteOperation = TableOperation.Delete(alertEntity);

            // Execute the delete operation.
            try
            {
                alertsTable.Execute(deleteOperation);
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
            var alertsForThisEmail = GetAllAlerts(new JobAlertsQuery { JobsSet = jobsSet, EmailAddress = emailAddress });
            if (alertsForThisEmail.Any()) return;

            // Get the storage account connection
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);

            // Delete the record of alerts sent
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("JobAlertsSent");
            table.CreateIfNotExistsAsync().Wait();

            TableQuery<TableEntity> tableQuery = new TableQuery<TableEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, emailAddress))
                .Where(TableQuery.GenerateFilterCondition("JobsSet", QueryComparisons.Equal, jobsSet.ToString()));

            var alertsSentEntities = table.ExecuteQuery(tableQuery);
            foreach (var entity in alertsSentEntities)
            {
                var operation = TableOperation.Delete(entity);
                try
                {
                    table.Execute(operation);
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

        public JobAlert GetAlertById(string alertId)
        {
            JobAlertTableEntity entity = GetTableEntityByRowKey(alertId);
            if (entity != null)
            {
                var searchQuery = HttpUtility.ParseQueryString(String.IsNullOrEmpty(entity.Criteria) ? String.Empty : entity.Criteria);
                return new JobAlert()
                {
                    AlertId = entity.RowKey,
                    Query = _queryConverter.ToQuery(searchQuery),
                    Email = entity.PartitionKey,
                    Frequency = entity.Frequency,
                    JobsSet = (JobsSet)Enum.Parse(typeof(JobsSet), entity.JobsSet)
                };
            }

            return null;
        }

        private static JobAlertTableEntity GetTableEntityByRowKey(string alertId)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("JobAlerts");

            TableQuery<JobAlertTableEntity> tableQuery = new TableQuery<JobAlertTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, alertId));

            var entity = table.ExecuteQuery(tableQuery).FirstOrDefault();
            return entity;
        }

        // From http://stackoverflow.com/questions/14859405/azure-table-storage-returns-400-bad-request
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

        public void MarkAlertAsSent(JobsSet jobsSet, string emailAddress, int jobId)
        {
            // Get the storage account connection
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);

            // Create the table if it doesn't exist.
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("JobAlertsSent");
            table.CreateIfNotExistsAsync().Wait();

            // Azure tables use an index clustered first by partition key then by row key.
            var entity = new JobAlertSentTableEntity()
            {
                PartitionKey = ToAzureKeyString(emailAddress),
                RowKey = jobId.ToString(),
                JobsSet = jobsSet.ToString()
            };

            // Create the TableOperation object that inserts the entity.
            var insertOperation = TableOperation.InsertOrReplace(entity);

            // Execute the insert operation.
            try
            {
                var result = table.Execute(insertOperation);
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

        public IList<int> GetJobsSentForEmail(JobsSet jobsSet, string emailAddress)
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
            var table = tableClient.GetTableReference("JobAlertsSent");

            // Initialize a default TableQuery to retrieve all the entities in the table.
            TableQuery<TableEntity> tableQuery = new TableQuery<TableEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, emailAddress))
                .Where(TableQuery.GenerateFilterCondition("JobsSet", QueryComparisons.Equal, jobsSet.ToString()));

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;

            var jobIds = new List<int>();

            do
            {
                // Retrieve a segment (up to 1,000 entities).
                TableQuerySegment<TableEntity> tableQueryResult = Task.Run(() => table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken)).Result;

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;

                foreach (var entity in tableQueryResult.Results)
                {
                    jobIds.Add(Int32.Parse(entity.RowKey));
                }

                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);

            return jobIds;
        }
    }
}