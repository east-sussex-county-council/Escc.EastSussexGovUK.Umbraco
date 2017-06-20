using System;
using System.Text;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Exceptionless;
using System.Configuration;
using Umbraco.Core.Logging;
using System.Security.Cryptography;

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
            //
            // When we look up a keyword we will want matching search terms ordered by page views.
            // To get that we need to have results ordered by page views, then filter the list to only search terms that match, 
            // which means the partition key has to be based on page views and the row key based on search terms.
            //
            // The partition key is a string, so convert the number of page views to a string and pad with leading 0s so that
            // the alpha sort gives the same result as a numeric sort. However this still sorts low numbers of page views ahead
            // of high, so we need to change low numbers to high ones and vice versa to get the right sort order. Subtracting 1000000
            // makes the numbers of page views negative (assuming they're under 1000000), and multiplying by -1 removes the minus sign,
            // giving us the sort order we want.
            //
            // The row key has to be a sanitised version of the keyword because a key can't contain common search term characters such 
            // as / and ?, so save the keyword separately as-typed so that it can be presented back to users.
            var entity = new JobAlertTableEntity()
            {
                PartitionKey = ToAzureKeyString(alert.Email),
                RowKey = HashIt(alert.Criteria),
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

        public string HashIt(string criteria)
        {
            HashAlgorithm algorithm = SHA1.Create();
            var bytes = Encoding.ASCII.GetBytes(criteria ?? String.Empty);
            var hash = algorithm.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}