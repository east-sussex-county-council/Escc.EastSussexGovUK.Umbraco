using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Escc.Umbraco.PropertyTypes;
using Exceptionless;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Web.WebApi
{
    /// <summary>
    /// API for returning URLs where web chat should be activated
    /// </summary>
    public class WebChatController : UmbracoApiController
    {
        /// <summary>
        /// Gets the urls where web chat should or should not be active.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Do NOT add a using statement around the response - it causes a 500 error with no error message to debug it</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public async Task<HttpResponseMessage> GetWebChatUrls()
        {
            try
            {
                var service = new UmbracoWebChatSettingsService(UmbracoContext.ContentCache.GetAtRoot().FirstOrDefault(), new UrlListReader());
                var settings = await service.ReadWebChatSettings();

                var response = Request.CreateResponse(HttpStatusCode.OK, settings);
                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromDays(1)
                };
                response.Content.Headers.Expires = DateTimeOffset.Now.Add(response.Headers.CacheControl.MaxAge.Value);

                return response;
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}