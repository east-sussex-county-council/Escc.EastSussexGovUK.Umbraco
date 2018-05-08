using System;
using Escc.EastSussexGovUK.Umbraco.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.Guide;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task;
using Escc.EastSussexGovUK.Umbraco.Location;
using Umbraco.Inception.Attributes;
using Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits;
using Escc.EastSussexGovUK.Umbraco.Forms;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map
{

    [UmbracoContentType("Map", "Map", new Type[] { 
        typeof(LandingPageWithPicturesDocumentType), 
        typeof(TaskDocumentType), 
        typeof(LandingDocumentType), 
        typeof(LocationDocumentType), 
        typeof(PersonDocumentType),
        typeof(GuideDocumentType),
        typeof(StandardLandingPageDocumentType),
        typeof(StandardTopicPageDocumentType), 
        typeof(StandardDownloadPageDocumentType),
        typeof(MapDocumentType), 
        typeof(FormDownloadDocumentType),
        typeof(ChildcareDocumentType),
        typeof(CouncilOfficeDocumentType),
        typeof(DayCentreDocumentType),
        typeof(LibraryDocumentType),
        typeof(ParkDocumentType),
        typeof(RecyclingSiteDocumentType),
        typeof(RegistrationOfficeDocumentType),
        typeof(SportLocationDocumentType),
        typeof(CampaignLandingDocumentType),
        typeof(CouncilPlanHomePageDocumentType),
        typeof(RightsOfWayDepositsDocumentType),
        typeof(FormDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconGlobeInvertedEuropeAfrica, "", false, false, 
    Description = "A clickable map which links to a set of pages. For a service based in one place (eg a library), use 'Location' or a more specific template instead or insert a Google Map into any page.")]
    public class MapDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public MapContentTab Content { get; set; }
    }
}