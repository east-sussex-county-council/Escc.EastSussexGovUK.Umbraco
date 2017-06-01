using Escc.EastSussexGovUK.Umbraco.Services;
using Umbraco.Web;
using Umbraco.Core.Models;
using Escc.AddressAndPersonalDetails;
using Escc.Geo;
using System;
using Escc.Dates;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    public class RightsOfWayDepositViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<RightsOfWayDepositViewModel>
    {
        public RightsOfWayDepositViewModelFromUmbraco(IPublishedContent umbracoContent) : base(umbracoContent, null, null)
        {
        }

        public RightsOfWayDepositViewModel BuildModel()
        {
            var model = new RightsOfWayDepositViewModel();
            model.Reference = UmbracoContent.Name;

            model.Owner = new PersonName();
            model.Owner.Titles.Add(UmbracoContent.GetPropertyValue<string>("HonorificTitle_Content"));
            model.Owner.GivenNames.Add(UmbracoContent.GetPropertyValue<string>("GivenName_Content"));
            model.Owner.FamilyName = UmbracoContent.GetPropertyValue<string>("FamilyName_Content");
            model.Owner.Suffixes.Add(UmbracoContent.GetPropertyValue<string>("HonorificSuffix_Content"));

            var addressInfo = UmbracoContent.GetPropertyValue<AddressInfo>("location_Content");
            model.Address = addressInfo.BS7666Address;
            if (addressInfo.GeoCoordinate != null)
            {
                model.Coordinates = new LatitudeLongitude(addressInfo.GeoCoordinate.Latitude, addressInfo.GeoCoordinate.Longitude);
            }
            model.OrdnanceSurveyGridReference = UmbracoContent.GetPropertyValue<string>("GridReference_Content");

            model.DateDeposited = UmbracoContent.GetPropertyValue<DateTime>("DateDeposited_Content");
            model.Metadata.DateIssued = model.DateDeposited.ToIso8601Date();

            var depositDocument = UmbracoContent.GetPropertyValue<IPublishedContent>("DepositDocument_Content");
            if (depositDocument != null && !String.IsNullOrEmpty(depositDocument.Url))
            {
                model.DepositUrl = new Uri(depositDocument.Url, UriKind.Relative);
            }

            model.Parish = UmbracoContent.GetPropertyValue<string>("Parish_Content");
            model.Metadata.Description = UmbracoContent.GetPropertyValue<string>("pageDescription_Content");
            return model;
        }
    }
}