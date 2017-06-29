using Escc.EastSussexGovUK.Umbraco.Services;
using Umbraco.Web;
using Umbraco.Core.Models;
using Escc.AddressAndPersonalDetails;
using Escc.Geo;
using System;
using Escc.Dates;
using System.Collections.Generic;
using Escc.Umbraco.PropertyTypes;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Gets details of an individual rights of way Section 31 deposit from an instance of the "RightsOfWayDeposit" Umbraco document type
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.BaseViewModelFromUmbracoBuilder" />
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits.RightsOfWayDepositViewModel}" />
    public class RightsOfWayDepositViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<RightsOfWayDepositViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightsOfWayDepositViewModelFromUmbraco"/> class.
        /// </summary>
        /// <param name="umbracoContent">Content of the umbraco.</param>
        public RightsOfWayDepositViewModelFromUmbraco(IPublishedContent umbracoContent) : base(umbracoContent, null, null)
        {
        }

        /// <summary>
        /// Gets the view model based on Umbraco properties.
        /// </summary>
        /// <returns></returns>
        public RightsOfWayDepositViewModel BuildModel()
        {
            var model = new RightsOfWayDepositViewModel();
            model.Reference = UmbracoContent.Name;

            model.Owner = new PersonName();
            model.Owner.Titles.Add(UmbracoContent.GetPropertyValue<string>("HonorificTitle_Content"));
            model.Owner.GivenNames.Add(UmbracoContent.GetPropertyValue<string>("GivenName_Content"));
            model.Owner.FamilyName = UmbracoContent.GetPropertyValue<string>("FamilyName_Content");
            model.Owner.Suffixes.Add(UmbracoContent.GetPropertyValue<string>("HonorificSuffix_Content"));

            var addressInfo = UmbracoContent.GetPropertyValue<AddressInfo>("Location_Content");
            model.Address = addressInfo.BS7666Address;
            if (addressInfo.GeoCoordinate != null)
            {
                model.Coordinates = new LatitudeLongitude(addressInfo.GeoCoordinate.Latitude, addressInfo.GeoCoordinate.Longitude);
            }
            model.OrdnanceSurveyGridReference = UmbracoContent.GetPropertyValue<string>("GridReference_Content");

            model.DateDeposited = UmbracoContent.GetPropertyValue<DateTime>("DateDeposited_Content");
            model.Metadata.DateIssued = model.DateDeposited.ToIso8601Date();

            var depositDocuments = UmbracoContent.GetPropertyValue<IEnumerable<IPublishedContent>>("DepositDocument_Content");
            if (depositDocuments != null)
            {
                foreach (var depositDocument in depositDocuments)
                {
                    model.DepositDocuments.Add(new HtmlLink()
                    {
                        Url = new Uri(depositDocument.Url, UriKind.Relative),
                        Text = depositDocument.Name
                    });
                }
            }

            var parishData = UmbracoContent.GetPropertyValue<string>("Parish_Content");
            if (!String.IsNullOrEmpty(parishData))
            {
                var parishes = parishData.Split(',');
                foreach (var parish in parishes)
                {
                    model.Parishes.Add(parish);
                }
            }

            model.Metadata.Description = UmbracoContent.GetPropertyValue<string>("pageDescription_Content");
            return model;
        }
    }
}