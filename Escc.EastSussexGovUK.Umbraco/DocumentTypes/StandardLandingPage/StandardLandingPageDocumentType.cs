using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage
{
    [UmbracoContentType("Standard landing page", "standardLandingPage", new Type[] { 
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

