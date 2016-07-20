using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage
{
    [UmbracoContentType("Standard download page", "standardDownloadPage", new Type[] { 
        typeof(LandingPageWithPicturesDocumentType), 
        typeof(TaskDocumentTypeAlias), 
        typeof(LandingDocumentTypeAlias), 
        typeof(LocationDocumentTypeAlias), 
        typeof(PersonDocumentType),
        typeof(GuideDocumentTypeAlias),
        typeof(StandardLandingPageDocumentType),
        typeof(StandardTopicPageDocumentType), 
        typeof(StandardDownloadPageDocumentType),
        typeof(MapDocumentType), 
        typeof(FormDownloadDocumentType),
        typeof(ChildcareDocumentTypeAlias),
        typeof(CouncilOfficeDocumentTypeAlias),
        typeof(DayCentreDocumentTypeAlias),
        typeof(LibraryDocumentTypeAlias),
        typeof(MobileLibraryStopDocumentTypeAlias),
        typeof(ParkDocumentTypeAlias),
        typeof(RecyclingSiteDocumentTypeAlias),
        typeof(RegistrationOfficeDocumentTypeAlias),
        typeof(SportLocationDocumentTypeAlias),
        typeof(CampaignLandingDocumentType),
        typeof(CouncilPlanHomePageDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconPageDown, "", false, false,
    Description = "A set of downloadable documents, optionally in two formats. It's usually better to link to documents within other pages instead.")]
    public class StandardDownloadPageDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public StandardDownloadPageContentTab Content { get; set; }
    }
}

