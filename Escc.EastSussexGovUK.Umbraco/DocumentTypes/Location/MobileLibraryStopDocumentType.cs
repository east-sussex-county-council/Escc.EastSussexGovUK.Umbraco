using System;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location
{
    /// <summary>
    /// An Umbraco document type for a mobile library stop, which gets most of its properties from the base <see cref="Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location"/> data type
    /// </summary>
    [UmbracoContentType("Mobile library stop", "MobileLibraryStop", new Type[]
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
        }, 
        true, MasterTemplate = "Location", Icon = BuiltInUmbracoContentTypeIcons.IconTruck,
        Description = "A time and place where a mobile library stops allowing residents to use council services, including borrowing books.")]
    public class MobileLibraryStopDocumentType : LocationDocumentType
    {
    }
}