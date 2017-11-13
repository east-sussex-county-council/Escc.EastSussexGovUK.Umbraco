using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.AddressAndPersonalDetails;
using Escc.EastSussexGovUK.Umbraco.Location;
using Exceptionless;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.WebApi
{
    /// <summary>
    /// Gets JSON data about locations saved using the Location template or one of its derivatives
    /// </summary>
    public class LocationController : UmbracoApiController
    {
        /// <summary>
        /// Lists locations filtered by the specified location types
        /// </summary>
        /// <param name="type">The types.</param>
        /// <param name="acceptsWaste">If present, matching locations must accept one or more of these waste types.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), AcceptVerbs("GET")]
        public HttpResponseMessage List([FromUri] string[] type, [FromUri] string[] acceptsWaste)
        {
            if (type.Length == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                var model = new List<LocationApiResult>();
                var types = new List<string>(type);
                var requiredWasteTypes = new List<string>(acceptsWaste);

                var homePage = Umbraco.TypedContentAtRoot().FirstOrDefault(child => child.DocumentTypeAlias == "HomePage");
                if (homePage != null)
                {
                    // Match on the document type alias
                    var locationPages = homePage.Descendants().Where(child => types.Contains(child.DocumentTypeAlias));

                    // Match on the accepted waste type - any one of the required waste types is enough
                    if (requiredWasteTypes.Count > 0)
                    {
                        locationPages = locationPages.Where(content =>
                        {
                            var allowedWasteTypes = new List<string>();

                            var recycledWasteTypes = content.GetPropertyValue<string>("wasteTypes_Content");
                            if (!String.IsNullOrEmpty(recycledWasteTypes))
                            {
                                allowedWasteTypes.AddRange(recycledWasteTypes.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries));
                            }

                            var nonRecycledWasteTypes = content.GetPropertyValue<string>("acceptedWasteTypes_Content");
                            if (!String.IsNullOrEmpty(nonRecycledWasteTypes))
                            {
                                allowedWasteTypes.AddRange(nonRecycledWasteTypes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                            }

                            return requiredWasteTypes.Any(wasteType => allowedWasteTypes.Contains(wasteType, StringComparer.OrdinalIgnoreCase));
                        });
                    }

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