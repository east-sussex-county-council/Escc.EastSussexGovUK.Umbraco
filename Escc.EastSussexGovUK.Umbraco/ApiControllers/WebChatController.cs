using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.PropertyTypes;
using Exceptionless;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.ApiControllers
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
        public HttpResponseMessage GetWebChatUrls()
        {
            try
            {
                var service = new UmbracoWebChatSettingsService(UmbracoContext.ContentCache.GetAtRoot().FirstOrDefault(), new UrlListReader());
                var settings = service.ReadWebChatSettings();

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