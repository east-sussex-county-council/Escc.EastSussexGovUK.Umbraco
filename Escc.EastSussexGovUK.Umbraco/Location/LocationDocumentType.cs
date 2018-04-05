using System;
using Escc.EastSussexGovUK.Umbraco.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.Guide;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task;
using Umbraco.Inception.Attributes;
using Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits;
using Escc.EastSussexGovUK.Umbraco.Forms;

namespace Escc.EastSussexGovUK.Umbraco.Location
{
    [UmbracoContentType("Location", "location", new Type[]
    {
        typeof(LandingDocumentType), 
        typeof(LocationDocumentType),
        typeof(TaskDocumentType), 
        typeof(LandingPageWithPicturesDocumentType),
        typeof(GuideDocumentType),
        typeof(StandardLandingPageDocumentType),
        typeof(StandardTopicPageDocumentType),
        typeof(StandardDownloadPageDocumentType),
        typeof(MapDocumentType),
        typeof(FormDownloadDocumentType),
        typeof(RecyclingSiteDocumentType),
        typeof(LibraryDocumentType),
        typeof(MobileLibraryStopDocumentType),
        typeof(ChildcareDocumentType),
        typeof(CouncilOfficeDocumentType),
        typeof(SportLocationDocumentType),
        typeof(ParkDocumentType),
        typeof(RegistrationOfficeDocumentType),
        typeof(DayCentreDocumentType),
        typeof(CampaignLandingDocumentType),
        typeof(PersonDocumentType),
        typeof(RightsOfWayDepositsDocumentType),
        typeof(FormDocumentType)
    }, true, icon: BuiltInUmbracoContentTypeIcons.IconPushpin, allowAtRoot: false, 
    Description = "A fixed location where the council delivers one or more services. Before using this, check for a more specific type such as 'Library' or 'Recycling site'.")]
    public class LocationDocumentType : CustomerFocusBaseDocumentType
    {
        [UmbracoTemplate(DisplayName="Location as vCard", Alias = "LocationVCard")]
        public string LocationVCard { get; set; }

        [UmbracoTab("Content")]
        public LocationContentTab Content { get; set; }
    }
}