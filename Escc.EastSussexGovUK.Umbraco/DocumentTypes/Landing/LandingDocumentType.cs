using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing
{
    [UmbracoContentType("Landing", "Landing", new Type[]
    {
        typeof(LandingDocumentTypeAlias),
        typeof(LocationDocumentTypeAlias), 
        typeof(TaskDocumentTypeAlias), 
        typeof(LandingPageWithPicturesDocumentTypeAlias),
        typeof(GuideDocumentTypeAlias),
        typeof(StandardLandingPageDocumentTypeAlias),
        typeof(StandardTopicPageDocumentTypeAlias),
        typeof(StandardDownloadPageDocumentTypeAlias),
        typeof(MapDocumentTypeAlias),
        typeof(FormDownloadPageDocumentTypeAlias),
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