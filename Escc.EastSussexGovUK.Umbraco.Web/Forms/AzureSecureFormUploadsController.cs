using Escc.Umbraco.Forms.Security;
using Exceptionless;
using Our.Umbraco.FileSystemProviders.Azure;
using System;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core.IO;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Web.Forms
{
    /// <summary>
    /// A secure API to download files uploaded using Umbraco Forms only if the current user has access to the form
    /// </summary>
    /// <remarks>Part of a workaround for http://issues.umbraco.org/issue/CON-1454 </remarks>
    /// <seealso cref="Umbraco.Web.WebApi.UmbracoAuthorizedApiController" />
    public class AzureSecureFormUploadsController : SecureFormUploadsController
    {
        protected override ActionResult ViewFile(string formId, string fileId, string filename, IFileSystem fileSystem)
        {
            try
            {
                var file = $"forms/upload/form_{formId}/{fileId}/{filename}";
                var azureFileSystem = (AzureBlobFileSystem)fileSystem;
                return new FileFromBlobStorageResult(azureFileSystem.ConnectionString, azureFileSystem.ContainerNameForUmbracoFormsUploads, file);
            }
            catch (Exception exception)
            {
                exception.ToExceptionless().Submit();
                throw;
            }
        }

        protected override ActionResult DownloadFile(string formId, string fileId, string filename, IFileSystem fileSystem)
        {
            try
            {
                HttpContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
            }
            catch (Exception exception)
            {
                exception.ToExceptionless().Submit();
                throw;
            }
            return ViewFile(formId, fileId, filename, fileSystem);
        }
    }
}
