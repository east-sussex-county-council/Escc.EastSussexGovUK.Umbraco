using Escc.FindAddress.Mvc;
using Escc.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Forms
{
    /// <summary>
    /// An Umbraco-compatible facade for <see cref="Escc.FindAddress.Mvc.FindAddressController" /> as there is no route to a standard Web API in an Umbraco application
    /// </summary>
    /// <seealso cref="Umbraco.Web.WebApi.UmbracoApiController" />
    [CorsPolicyFromConfig]
    public class FindAddressApiController : UmbracoApiController
    {
        /// <summary>
        /// Returns an object with a <c>data</c> property that contains a list of addresses matching the given postcode
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns></returns>
        [HttpGet]
        public object AddressesMatchingPostcode(string postcode)
        {
            return new FindAddressController().AddressesMatchingPostcode(postcode);
        }

        /// <summary>
        /// Returns an object with a <c>data</c> property that contains an address matching the given postcode and UPRN
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <param name="uprn">The UPRN.</param>
        /// <returns></returns>
        [HttpGet]
        public object AddressMatchingPostcodeAndUprn(string postcode, string uprn)
        {
            return new FindAddressController().AddressMatchingPostcodeAndUprn(postcode, uprn);
        }
    }
}