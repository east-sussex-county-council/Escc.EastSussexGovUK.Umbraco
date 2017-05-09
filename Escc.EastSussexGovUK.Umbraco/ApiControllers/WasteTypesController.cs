using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.EastSussexGovUK.Umbraco.Location;
using Exceptionless;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.ApiControllers
{
    /// <summary>
    /// Gets JSON data about waste types supported by the Recycling site document type
    /// </summary>
    public class WasteTypesController : UmbracoApiController
    {
        /// <summary>
        /// Lists all waste types
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), AcceptVerbs("GET")]
        public HttpResponseMessage List()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, WasteTypesDataType.WasteTypes);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}