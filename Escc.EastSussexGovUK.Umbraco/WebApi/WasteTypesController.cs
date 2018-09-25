using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.EastSussexGovUK.Umbraco.Location;
using Exceptionless;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.WebApi
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
                return Request.CreateResponse(HttpStatusCode.OK, WasteTypes);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private IEnumerable<string> WasteTypes = new string[] {
            "Aluminium foil",
            "Aerosols",
            "Bonded asbestos",
            "Books",
            "Bric-a-brac",
            "Car batteries",
            "Cans/Tins",
            "Cardboard",
            "Cassettes",
            "CDs and cases",
            "Chemicals",
            "Cooking oil",
            "Electrical goods",
            "Engine oil",
            "Fluorescent tubes/Energy saving bulbs",
            "Furniture",
            "Fridges/Freezers",
            "Glass bottles/Jars",
            "Green garden waste/Christmas trees",
            "Hard plastics (for example, plastic toys and furniture)",
            "Hardcore/Rubble",
            "Household batteries",
            "Household waste",
            "Metal items",
            "Mobile phones",
            "Newspapers/Magazines/Junk mail/White Telephone Directories",
            "Paint – solvent-based",
            "Paint – water-based emulsion",
            "Plasterboard",
            "Plastic bottles",
            "Plastic carrier bags",
            "Printer cartridges",
            "Soil",
            "Spectacles",
            "Tetra paks/Food or drink cartons",
            "Textiles/Shoes",
            "Timber/Wood",
            "Trade/Business waste",
            "TVs/Computer monitors",
            "Tyres",
            "Yellow Pages"
        };
    }
}