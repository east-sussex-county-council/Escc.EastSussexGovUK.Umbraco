using Escc.EastSussexGovUK.Umbraco.Web.Services;
using Umbraco.Web;
using Umbraco.Core.Models;
using Escc.AddressAndPersonalDetails;
using Escc.Geo;
using System;
using Escc.Dates;
using System.Collections.Generic;
using Escc.Umbraco.PropertyTypes;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections;

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayDeposits
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

            for (var i = 1; i <= 5; i++)
            {
                var owner = UmbracoContent.GetPropertyValue<PersonName>($"Owner{i}_Content");
                if (owner != null) { model.IndividualOwners.Add(owner); }

                var org = UmbracoContent.GetPropertyValue<string>($"OrganisationalOwner{i}_Content");
                if (!String.IsNullOrEmpty(org)) { model.OrganisationalOwners.Add(org); }

                var addressInfo = UmbracoContent.GetPropertyValue<AddressInfo>($"Location{((i > 1) ? i.ToString(CultureInfo.InvariantCulture) : String.Empty)}_Content");
                if (addressInfo != null && addressInfo.BS7666Address.HasAddress() && addressInfo.BS7666Address.ToString() != addressInfo.BS7666Address.AdministrativeArea)
                {
                    if (addressInfo.GeoCoordinate.Latitude == 0 && addressInfo.GeoCoordinate.Longitude == 0) addressInfo.GeoCoordinate = null;
                    model.Addresses.Add(addressInfo);
                }
            }

            model.OrdnanceSurveyGridReference = UmbracoContent.GetPropertyValue<string>("GridReference_Content");

            model.DateDeposited = UmbracoContent.GetPropertyValue<DateTime>("DateDeposited_Content");
            model.Metadata.DateIssued = model.DateDeposited;
            model.DateExpires = UmbracoContent.GetPropertyValue<DateTime>("DateExpires_Content");

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
            
            var parishes = UmbracoContent.GetPropertyValue<IEnumerable<string>>("Parish_Content");
            foreach (var parish in parishes)
            {
                model.Parishes.Add(parish);
            }

            model.Description = UmbracoContent.GetPropertyValue<string>("pageDescription_Content");

            return model;
        }
    }
}