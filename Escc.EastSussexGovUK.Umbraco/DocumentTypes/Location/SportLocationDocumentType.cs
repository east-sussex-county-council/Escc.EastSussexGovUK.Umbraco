using System;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location
{
    /// <summary>
    /// An Umbraco document type for a sport venue, which gets most of its properties from the base <see cref="Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location"/> data type
    /// </summary>
    [UmbracoContentType("Sport venue", "SportLocation", new Type[]
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
        true, MasterTemplate = "Location", Icon = BuiltInUmbracoContentTypeIcons.IconTShirt,
        Description = "A sport venue, such as a playing field or watersports centre.")]
    public class SportLocationDocumentType : LocationDocumentType
    {
    }
}