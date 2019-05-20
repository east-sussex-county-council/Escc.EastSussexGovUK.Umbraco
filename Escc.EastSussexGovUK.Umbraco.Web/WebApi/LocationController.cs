using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.AddressAndPersonalDetails;
using Escc.EastSussexGovUK.Umbraco.Web.Location;
using Exceptionless;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using Escc.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.WebApi
{
    /// <summary>
    /// Gets JSON data about locations saved using the Location template or one of its derivatives
    /// </summary>
    [CorsPolicyFromConfig]
    public class LocationController : UmbracoApiController
    {
        /// <summary>
        /// Lists locations filtered by the specified location types
        /// </summary>
        /// <param name="type">The types.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), AcceptVerbs("GET")]
        public HttpResponseMessage List([FromUri] string[] type)
        {
            if (type.Length == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                var model = new List<LocationApiResult>();
                var types = new List<string>(type);

                var homePage = Umbraco.TypedContentAtRoot().FirstOrDefault(child => child.DocumentTypeAlias == "HomePage");
                if (homePage != null)
                {
                    // Match on the document type alias
                    var locationPages = homePage.Descendants().Where(child => types.Contains(child.DocumentTypeAlias));

                    // Return a result for each matching template
                    model.AddRange(locationPages.Select(content =>
                    {
                        var ukLocation = content.GetPropertyValue<AddressInfo>("location_Content");
                        double latitude = (ukLocation != null) ? ukLocation.GeoCoordinate.Latitude : 0;
                        double longitude = (ukLocation != null) ? ukLocation.GeoCoordinate.Longitude : 0;
                        var town = (ukLocation != null) ? ukLocation.BS7666Address.Town : String.Empty;
                        return new LocationApiResult()
                        {
                            Name = content.Name,
                            Description = content.GetPropertyValue<string>("pageDescription"),
                            Url = new Uri(content.UrlWithDomain()),
                            LocationType = content.DocumentTypeAlias,
                            Latitude = latitude,
                            Longitude = longitude,
                            Town = town
                        };
                    }));
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}