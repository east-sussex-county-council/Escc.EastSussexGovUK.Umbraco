using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person
{
    /// <summary>
    /// An Umbraco document type representing a person
    /// </summary>
    /// <remarks>The display name is ESCC employee as a hint for its expected use, and because the default view encodes that in the metadata. 
    /// Alias left as 'Person' to allow extension to specific types, similar to 'Location'.</remarks>
    /// <seealso cref="CustomerFocusBaseDocumentType" />
    [UmbracoContentType("ESCC employee", "Person", new Type[]
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
    }, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconUserFemale, 
    Description = "A profile of an ESCC employee and their role, such as the Chief Executive.")]
    public class PersonDocumentType : CustomerFocusBaseDocumentType
    {
        [UmbracoTab("Content")]
        public PersonContentTab Content { get; set; }
    }
}