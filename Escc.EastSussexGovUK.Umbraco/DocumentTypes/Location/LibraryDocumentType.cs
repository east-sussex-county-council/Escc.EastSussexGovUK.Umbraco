using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location
{
    /// <summary>
    /// An Umbraco document type for a branch library, which gets most of its properties from the base <see cref="Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location"/> data type
    /// </summary>
    [UmbracoContentType("Library", "Library", new Type[]
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
        }, 
        true, MasterTemplate = "Location", Icon = BuiltInUmbracoContentTypeIcons.IconBooks,
        Description = "A branch library where residents can use council services, including borrowing books.")]
    public class LibraryDocumentType : LocationDocumentType
    {
    }
}