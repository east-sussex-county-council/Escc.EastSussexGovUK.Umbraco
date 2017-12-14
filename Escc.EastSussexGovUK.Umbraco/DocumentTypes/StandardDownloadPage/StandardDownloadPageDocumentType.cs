using System;
using Escc.EastSussexGovUK.Umbraco.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Guide;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task;
using Escc.EastSussexGovUK.Umbraco.Location;
using Umbraco.Inception.Attributes;
using Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits;
using Escc.EastSussexGovUK.Umbraco.Forms;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage
{
    [UmbracoContentType("Standard download page", "standardDownloadPage", new Type[] { 
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
        typeof(RightsOfWayDepositsDocumentType),
        typeof(FormDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconPageDown, "", false, false,
    Description = "A set of downloadable documents, optionally in two formats. It's usually better to link to documents within other pages instead.")]
    public class StandardDownloadPageDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public StandardDownloadPageContentTab Content { get; set; }
    }
}

