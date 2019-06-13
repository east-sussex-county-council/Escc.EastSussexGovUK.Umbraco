using Escc.AddressAndPersonalDetails;
using Escc.EastSussexGovUK.Umbraco.Web.Models;
using Escc.Geo;
using Escc.Umbraco.PropertyTypes;
using System;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayModifications
{
    /// <summary>
    /// Details of a single rights of way definitive map modification order application to be displayed on a detail page for that application
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Models.BaseViewModel" />
    public class RightsOfWayModificationViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the reference number for the application in the form ROW123
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets the individuals who wholly or partly own the land affected by the claim
        /// </summary>
        public IList<PersonName> IndividualOwners { get; private set; } = new List<PersonName>();

        /// <summary>
        /// Gets the groups or organisations which wholly or partly own the land affected by the claim
        /// </summary>
        public IList<string> OrganisationalOwners { get; private set; } = new List<string>();

        /// <summary>
        /// Gets or sets the location(s) affected by the claim
        /// </summary>
        public IList<AddressInfo> Addresses { get; } = new List<AddressInfo>();

        /// <summary>
        /// Gets or sets a comma-separated list of OS grid references affected by the claim
        /// </summary>
        public string OrdnanceSurveyGridReference { get; set; }

        /// <summary>
        /// Gets the parishes the affected land is wholly or partly within
        /// </summary>
        public IList<string> Parishes { get; private set; } = new List<string>();

        /// <summary>
        /// Gets or sets the nearest settlement to the site of the claim
        /// </summary>
        public string NearestTownOrVillage { get; set; }

        /// <summary>
        /// Gets or sets a description of the route of the claimed right of way
        /// </summary>
        public string DescriptionOfRoute { get; set; }

        /// <summary>
        /// Gets or sets whether the application claims footpath(s), bridleway(s), and restricted or unrestricted byway(s) 
        /// </summary>
        public string StatusClaimed { get; set; }

        /// <summary>
        /// Gets or sets the URL of the page with details of the application
        /// </summary>
        public Uri PageUrl { get; set; }

        /// <summary>
        /// Gets or sets any attached documents which support the application
        /// </summary>
        public IList<HtmlLink> ApplicationDocuments { get; private set; } = new List<HtmlLink>();

        /// <summary>
        /// Gets or sets the date the application was received by the rights of way team
        /// </summary>
        public DateTime DateReceived { get; set; }

        /// <summary>
        /// Gets or sets the status of the application
        /// </summary>
        public string ApplicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the date a decision was made on the application
        /// </summary>
        public DateTime? DateDetermined { get; set; }

        /// <summary>
        /// Gets or sets the name of the applicant if it is an individual
        /// </summary>
        public PersonName IndividualApplicant { get; set; }

        /// <summary>
        /// Gets or sets the name of the application if it is a group or organisation
        /// </summary>
        public string OrganisationalApplicant { get; set; }

        /// <summary>
        /// Gets or sets the name of the council officer assigned to deal with the application
        /// </summary>
        public PersonName CouncilOfficerAssigned { get; set; }

        /// <summary>
        /// Gets or sets the decision made
        /// </summary>
        public string Decision { get; set; }

        /// <summary>
        /// Gets or sets the date the definitive map modification order is confirmed following approval
        /// </summary>
        public DateTime? DateModificationOrderConfirmed { get; set; }
    }
}