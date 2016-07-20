using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Guide;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing
{
    [UmbracoContentType("Landing", "Landing", new Type[]
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
        typeof(PersonDocumentType)
    }, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconThumbnailsSmall,
    Description = "The menu for a section, with clickable links to the pages below.")]
    public class LandingDocumentType : CustomerFocusBaseDocumentType
    {
        /* Main Contents */
        [UmbracoTab("Content")]
        public LandingContentTab Content { get; set; }
    }
}