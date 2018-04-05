using System;
using Escc.EastSussexGovUK.Umbraco.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.Guide;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task;
using Umbraco.Inception.Attributes;
using Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits;
using Escc.EastSussexGovUK.Umbraco.Forms;

namespace Escc.EastSussexGovUK.Umbraco.Location
{
    /// <summary>
    /// An Umbraco document type for a council office, which gets most of its properties from the base <see cref="Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location"/> data type
    /// </summary>
    [UmbracoContentType("Council office", "CouncilOffice", new Type[]
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
        typeof(PersonDocumentType),
        typeof(RightsOfWayDepositsDocumentType),
        typeof(FormDocumentType)
        }, 
        true, MasterTemplate = "Location", Icon = BuiltInUmbracoContentTypeIcons.IconLibrary,
        Description = "An office where council staff are based.")]
    public class CouncilOfficeDocumentType : LocationDocumentType
    {
    }
}