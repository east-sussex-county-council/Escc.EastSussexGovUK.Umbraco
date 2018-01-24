using Our.Umbraco.FileSystemProviders.Azure;
using System.Web.Http;
using Umbraco.Core.IO;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Forms
{
    /// <summary>
    /// A secure API to download files uploaded using Umbraco Forms only if the current user has access to the form
    /// </summary>
    /// <remarks>Part of a workaround for http://issues.umbraco.org/issue/CON-1454 </remarks>
    /// <seealso cref="Umbraco.Web.WebApi.UmbracoAuthorizedApiController" />
    public class UmbracoFormsUploadsApiController : UmbracoAuthorizedApiController
    {
        /// <summary>
        /// Downloads the file if the current user has access to the form
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult DownloadFile(string file)
        {
            var fileUploads = new UmbracoFormsFileUploads(); 
            var formId = fileUploads.GetFormIdForUploadedFile(file);
            if (formId == null)
            {
                return NotFound();
            }

            var security = new UmbracoFormsSecurity();
            if (!security.UserHasAccessToForm(UmbracoContext.Security.CurrentUser.Id, formId.Value))
            {
                return Unauthorized();
            }

            var fileSystem = FileSystemProviderManager.Current.GetUnderlyingFileSystemProvider("media") as AzureBlobFileSystem;
            if (fileSystem == null)
            {
                return InternalServerError();
            }
            
            return new FileFromBlobStorageResult(fileSystem.ConnectionString, fileSystem.ContainerNameForUmbracoFormsUploads, file);
        }
    }
}
