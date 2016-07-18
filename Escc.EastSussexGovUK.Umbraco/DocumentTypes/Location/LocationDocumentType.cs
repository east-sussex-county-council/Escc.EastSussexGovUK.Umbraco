using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location
{
    [UmbracoContentType("Location", "location", new Type[]
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
        typeof(CampaignLandingDocumentTypeAlias),
        typeof(PersonDocumentTypeAlias)
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