using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Web.Http;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.Umbraco.Media;
using Exceptionless;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Web.WebApi
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
        /// <remarks>Do NOT add a using statement around the response - it causes a 500 error with no error message to debug it</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public HttpResponseMessage GetMediaOnPage([FromUri] int pageId)
        {
            try
            {
                // This API hits the database. Although Umbraco caches the result, we can cache it more aggressively.
                var resultsArray = MemoryCache.Default.Get($"GetMediaOnPage-{pageId}");
                if (resultsArray == null)
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
                            Extension = media.Values["umbracoExtension"],
                            Url = new Uri(media.Values["umbracoFile"], UriKind.Relative)
                        });
                    }
                    resultsArray = results.ToArray();
                    MemoryCache.Default.Add($"GetMediaOnPage-{pageId}", resultsArray, DateTimeOffset.UtcNow.AddDays(1));
                }

                // Cache at the HTTP level too.
                var response = Request.CreateResponse(HttpStatusCode.OK, resultsArray);
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