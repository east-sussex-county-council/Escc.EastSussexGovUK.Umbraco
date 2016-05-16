using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures
{

    [UmbracoContentType("Landing page with pictures", "landingPageWithPictures", new Type[] { 
        typeof(LandingPageWithPicturesDocumentType), 
        typeof(TaskDocumentTypeAlias), 
        typeof(LandingDocumentTypeAlias), 
        typeof(LocationDocumentTypeAlias), 
        typeof(PersonDocumentTypeAlias),
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
        typeof(CampaignLandingDocumentTypeAlias),
        typeof(CouncilPlanHomePageDocumentTypeAlias)
    }, true, BuiltInUmbracoContentTypeIcons.IconThumbnailList, "", false, false,
    Description = "A landing page with a picture on the right or above each link. You should usually use the 'Landing' template instead.")]
    public class LandingPageWithPicturesDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public LandingPageWithPicturesContentTab Content { get; set; }
    }
}