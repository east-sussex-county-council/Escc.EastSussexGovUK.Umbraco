using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload
{

    [UmbracoContentType("Form download page", "FormDownloadPage", new Type[] { 
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
    }, true, BuiltInUmbracoContentTypeIcons.IconAutofill, "", false, false, 
    Description = "Introduction page for a form. For new pages, use the 'Task' template instead.")]
    public class FormDownloadDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public FormDownloadContentTab Content { get; set; }
    }
}