using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Escc.EastSussexGovUK.Umbraco.Models;
using Escc.Umbraco.Media;
using Exceptionless;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.WebApi
{
    /// <summary>
    /// API for returning information about Umbraco media items
    /// </summary>
    public class MediaController : UmbracoApiController
    {
        /// <summary>
        /// Gets the media used on a page, as tracked when <see cref="Escc.Umbraco.MediaSync"/> is installed.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <returns></returns>
        public HttpResponseMessage GetMediaOnPage([FromUri] int pageId)
        {
            try
            {
                var results = new List<UmbracoMediaApiResult>();

                var uMediaSyncRelations = ApplicationContext.Services.RelationService.GetByParentId(pageId).Where(r => r.RelationType.Alias == "uMediaSyncFileRelation");
                var mediaItemIds = uMediaSyncRelations.Select(r => r.ChildId);
                foreach (var id in mediaItemIds)
                {
                    var media = MediaHelper.GetUmbracoMedia(id);
                    results.Add(new UmbracoMediaApiResult()
                    {
                        Size = MediaHelper.GetFileSizeInKilobytes(media),
                        Extension= media.Values["umbracoExtension"],
                        Url = new Uri(media.Values["umbracoFile"], UriKind.Relative)
                    });
                }

                // This API hits the database, although it caches the result, so cache at the HTTP level too.
                var response = Request.CreateResponse(HttpStatusCode.OK, results.ToArray());
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