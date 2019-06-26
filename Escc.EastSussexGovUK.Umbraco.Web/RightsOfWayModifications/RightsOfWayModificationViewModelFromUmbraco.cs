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

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayModifications
{
    /// <summary>
    /// Gets details of an individual rights of way definitive map modification order from an instance of the "RightsOfWayModification" Umbraco document type
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.BaseViewModelFromUmbracoBuilder" />
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{Escc.EastSussexGovUK.Umbraco.RightsOfWayModifications.RightsOfWayModificationViewModel}" />
    public class RightsOfWayModificationViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<RightsOfWayModificationViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightsOfWayModificationViewModelFromUmbraco"/> class.
        /// </summary>
        /// <param name="umbracoContent">Content of the umbraco.</param>
        public RightsOfWayModificationViewModelFromUmbraco(IPublishedContent umbracoContent) : base(umbracoContent, null, null)
        {
        }

        /// <summary>
        /// Gets the view model based on Umbraco properties.
        /// </summary>
        /// <returns></returns>
        public RightsOfWayModificationViewModel BuildModel()
        {
            var model = new RightsOfWayModificationViewModel
            {
                Reference = UmbracoContent.Name,
                NearestTownOrVillage = UmbracoContent.GetPropertyValue<string>("nearestTownOrVillage"),
                DescriptionOfRoute = UmbracoContent.GetPropertyValue<string>("pageDescription"),
                StatusClaimed = UmbracoContent.GetPropertyValue<string>("statusClaimed"),
                OrdnanceSurveyGridReference = UmbracoContent.GetPropertyValue<string>("GridReference"),
                IndividualApplicant = UmbracoContent.GetPropertyValue<PersonName>("nameOfApplicant"),
                OrganisationalApplicant = UmbracoContent.GetPropertyValue<string>("nameOfApplicantOrganisation"),
                CouncilOfficerAssigned = UmbracoContent.GetPropertyValue<PersonName>("councilOfficerAssigned"),
                ApplicationStatus= UmbracoContent.GetPropertyValue<string>("applicationStatus"),
                Decision = UmbracoContent.GetPropertyValue<string>("decision")
            };

            for (var i = 1; i <= 5; i++)
            {
                var org = UmbracoContent.GetPropertyValue<string>($"OrganisationalOwner{i}");
                if (!String.IsNullOrEmpty(org)) { model.OrganisationalOwners.Add(org); }

                var addressInfo = UmbracoContent.GetPropertyValue<AddressInfo>($"Location{i}");
                if (addressInfo != null && addressInfo.BS7666Address.HasAddress() && addressInfo.BS7666Address.ToString() != addressInfo.BS7666Address.AdministrativeArea)
                {
                    if (addressInfo.GeoCoordinate.Latitude == 0 && addressInfo.GeoCoordinate.Longitude == 0) addressInfo.GeoCoordinate = null;
                    model.Addresses.Add(addressInfo);
                }
            }


            model.DateReceived = UmbracoContent.GetPropertyValue<DateTime>("DateReceived");
            model.Metadata.DateCreated = model.DateReceived;
            model.DateDetermined = UmbracoContent.GetPropertyValue<DateTime>("DateDetermined");
            if (model.DateDetermined == DateTime.MinValue) { model.DateDetermined = null; }
            model.DateModificationOrderConfirmed = UmbracoContent.GetPropertyValue<DateTime>("orderConfirmedDate");
            if (model.DateModificationOrderConfirmed == DateTime.MinValue) { model.DateModificationOrderConfirmed = null; }

            var documents = UmbracoContent.GetPropertyValue<IEnumerable<IPublishedContent>>("Documents");
            if (documents != null)
            {
                foreach (var document in documents)
                {
                    model.ApplicationDocuments.Add(new HtmlLink()
                    {
                        Url = new Uri(document.Url, UriKind.Relative),
                        Text = document.Name
                    });
                }
            }
            
            var parishes = UmbracoContent.GetPropertyValue<IEnumerable<string>>("Parish");
            if (parishes != null)
            {
                foreach (var parish in parishes)
                {
                    model.Parishes.Add(parish);
                }
            }

            return model;
        }
    }
}