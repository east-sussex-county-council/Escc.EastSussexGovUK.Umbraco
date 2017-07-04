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

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    internal class AzureTableStorageAlertsRepository : IAlertsRepository
    {
        public void SaveAlert(JobAlert alert)
        {
            // Get the storage account connection
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);

            // Create the table if it doesn't exist.
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("jobalerts");
            table.CreateIfNotExistsAsync().Wait();

            // Azure tables use an index clustered first by partition key then by row key.
            var entity = new JobAlertTableEntity()
            {
                PartitionKey = ToAzureKeyString(alert.Email),
                RowKey = new JobAlertIdEncoder().GenerateId(alert),
                Criteria = alert.Criteria,
                Frequency = alert.Frequency
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
                    LogHelper.Error<AzureTableStorageAlertsRepository>(alert.Criteria + " returned " + ex.RequestInformation.ExtendedErrorInformation.ErrorMessage, ex);
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
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("jobalerts");
            table.CreateIfNotExistsAsync().Wait();

            var entity = GetTableEntityByRowKey(alertId);
            if (entity == null) return false;

            // Create the TableOperation object that deletes the entity.
            var deleteOperation = TableOperation.Delete(entity);

            // Execute the delete operation.
            try
            {
                var result = table.Execute(deleteOperation);
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
            return true;
        }

        public JobAlert GetAlertById(string alertId)
        {
            JobAlertTableEntity entity = GetTableEntityByRowKey(alertId);
            if (entity != null)
            {
                return new JobAlert()
                {
                    Criteria = entity.Criteria,
                    Email = entity.PartitionKey,
                    Frequency = entity.Frequency
                };
            }

            return null;
        }

        private static JobAlertTableEntity GetTableEntityByRowKey(string alertId)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["Escc.EastSussexGovUK.Umbraco.AzureStorage"].ConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("jobalerts");

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
    }
}