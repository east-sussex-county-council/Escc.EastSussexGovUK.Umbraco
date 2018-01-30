using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Escc.EastSussexGovUK.Umbraco.Forms
{
    /// <summary>
    /// Downloads a file from blob storage and returns it as the response from a Web API
    /// </summary>
    /// <remarks>Part of a workaround for http://issues.umbraco.org/issue/CON-1454.
    /// The blob code is adapted from https://github.com/JimBobSquarePants/UmbracoFileSystemProviders.Azure/</remarks>
    /// <seealso cref="System.Web.Http.IHttpActionResult" />
    class FileFromBlobStorageResult : IHttpActionResult
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFromBlobStorageResult"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="ArgumentNullException">filePath</exception>
        public FileFromBlobStorageResult(string connectionString, string containerName, string filePath)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("This parameter is required", nameof(connectionString));
            }

            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentException("This parameter is required", nameof(containerName));
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("This parameter is required", nameof(filePath));
            }

            _connectionString = connectionString;
            _containerName = containerName.ToLowerInvariant();
            _filePath = filePath;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            // Remove the container name from the URL, to get the path to the file within the container
            var blobPath = _filePath;
            var containerUrlPrefix = $"/{_containerName}/";
            if (blobPath.StartsWith(containerUrlPrefix))
            {
                blobPath = blobPath.Substring(containerUrlPrefix.Length);
            }

            // Get the reference to the file from the blob container
            CloudBlockBlob blockBlob = this.GetBlockBlobReference(blobPath);
            if (blockBlob == null || !blockBlob.Exists()) 
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            // Download the contents to a stream, and return that stream
            MemoryStream stream = new MemoryStream();
            blockBlob.DownloadToStream(stream);

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);

            var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(blobPath));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = Path.GetFileName(blobPath) };

            return Task.FromResult(response);
        }

        private CloudBlockBlob GetBlockBlobReference(string path)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = CreateContainer(cloudBlobClient, _containerName, BlobContainerPublicAccessType.Off);

            return !string.IsNullOrWhiteSpace(path) && cloudBlobContainer != null
               ? cloudBlobContainer.GetBlockBlobReference(path)
               : null;
        }


        /// <summary>
        /// Returns the media container
        /// </summary>
        /// <param name="cloudBlobClient"><see cref="CloudBlobClient"/> where the container is stored.</param>
        /// <param name="containerName">The name of the container.</param>
        /// <param name="accessType"><see cref="BlobContainerPublicAccessType"/> indicating the access permissions.</param>
        /// <returns>The <see cref="CloudBlobContainer"/></returns>
        private static CloudBlobContainer CreateContainer(CloudBlobClient cloudBlobClient, string containerName, BlobContainerPublicAccessType accessType)
        {
            // Validate container name - from: http://stackoverflow.com/a/23364534/5018
            Regex containerRegex = new Regex("^[a-z0-9](?:[a-z0-9]|(\\-(?!\\-))){1,61}[a-z0-9]$|^\\$root$", RegexOptions.Compiled);
            bool isContainerNameValid = containerRegex.IsMatch(containerName);
            if (isContainerNameValid == false)
            {
                throw new ArgumentException($"The container name {containerName} is not valid, see https://msdn.microsoft.com/en-us/library/azure/dd135715.aspx for the restrictions for container names.");
            }

            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);
            if (container != null) container.SetPermissions(new BlobContainerPermissions { PublicAccess = accessType });
            return container;
        }
    }
}
