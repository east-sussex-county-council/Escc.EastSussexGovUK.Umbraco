using System;
using Escc.EastSussexGovUK.Umbraco.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Guide;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task;
using Escc.EastSussexGovUK.Umbraco.Location;
using Umbraco.Inception.Attributes;
using Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures
{

    [UmbracoContentType("Landing page with pictures", "landingPageWithPictures", new Type[] { 
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
        typeof(MobileLibraryStopDocumentType),
        typeof(ParkDocumentType),
        typeof(RecyclingSiteDocumentType),
        typeof(RegistrationOfficeDocumentType),
        typeof(SportLocationDocumentType),
        typeof(CampaignLandingDocumentType),
        typeof(CouncilPlanHomePageDocumentType),
        typeof(RightsOfWayDepositsDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconThumbnailList, "", false, false,
    Description = "A landing page with a picture on the right or above each link. You should usually use the 'Landing' template instead.")]
    public class LandingPageWithPicturesDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public LandingPageWithPicturesContentTab Content { get; set; }
    }
}