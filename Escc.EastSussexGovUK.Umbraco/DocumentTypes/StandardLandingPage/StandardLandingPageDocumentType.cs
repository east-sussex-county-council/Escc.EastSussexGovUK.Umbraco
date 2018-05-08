using System;
using Escc.EastSussexGovUK.Umbraco.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.Guide;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task;
using Escc.EastSussexGovUK.Umbraco.Location;
using Umbraco.Inception.Attributes;
using Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits;
using Escc.EastSussexGovUK.Umbraco.Forms;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage
{
    [UmbracoContentType("Standard landing page", "standardLandingPage", new Type[] { 
        typeof(LandingPageWithPicturesDocumentType), 
        typeof(TaskDocumentType), 
        typeof(LandingDocumentType), 
        typeof(LocationDocumentType), 
        typeof(PersonDocumentType),
        typeof(GuideDocumentType),
        typeof(StandardLandingPageDocumentType),
        typeof(StandardTopicPageDocumentType), 
        typeof(StandardDownloadPageDocumentType),
        typeof(MapDocumentType), 
        typeof(FormDownloadDocumentType),
        typeof(ChildcareDocumentType),
        typeof(CouncilOfficeDocumentType),
        typeof(DayCentreDocumentType),
        typeof(LibraryDocumentType),
        typeof(ParkDocumentType),
        typeof(RecyclingSiteDocumentType),
        typeof(RegistrationOfficeDocumentType),
        typeof(SportLocationDocumentType),
        typeof(CampaignLandingDocumentType),
        typeof(CouncilPlanHomePageDocumentType),
        typeof(RightsOfWayDepositsDocumentType),
        typeof(FormDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconBulletedList, "", false, false, 
    Description = "Older-style landing page. For new pages, use the 'Landing' template instead.")]
    public class StandardLandingPageDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public StandardLandingPageContentTab Content { get; set; }

        [UmbracoProperty("Columns", "columns", BuiltInUmbracoDataTypes.DropDown, LandingPageColumnsDataType.DataTypeName, description: "'Auto' will use 3 columns when you use 9 or more sections.")]
        public string Columns { get; set; }

        [UmbracoProperty("Descriptions or links?", "descriptions", BuiltInUmbracoDataTypes.DropDown, LandingPageDescriptionsDataType.DataTypeName, description: "Description fields on the 'Content' tab are formatted based on this setting. Before saving, check they're the right format to avoid losing your work.\n\n'Auto' will use links at the level of the home page and its immediate children.")]
        public string Descriptions { get; set; }
    }
}

