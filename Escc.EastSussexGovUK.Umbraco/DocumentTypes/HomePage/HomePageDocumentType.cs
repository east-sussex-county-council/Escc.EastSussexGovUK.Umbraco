using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.Features.Latest;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.Features.SocialMedia;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.HomePage
{
    /// <summary>
    /// Specification for the 'Home page' document type in Umbraco
    /// </summary>
    [UmbracoContentType("Home page", "HomePage", new Type[]
    {
        typeof(LandingDocumentTypeAlias),
        typeof(TaskDocumentTypeAlias),
        typeof(LocationDocumentTypeAlias),
        typeof(PersonDocumentType),
        typeof(LandingPageWithPicturesDocumentTypeAlias),
        typeof(GuideDocumentTypeAlias),
        typeof(StandardLandingPageDocumentTypeAlias), 
        typeof(StandardTopicPageDocumentTypeAlias),
        typeof(StandardDownloadPageDocumentTypeAlias),
        typeof(MapDocumentTypeAlias),
        typeof(FormDownloadPageDocumentTypeAlias),
        typeof(HomePageItemsDocumentType),
        typeof(ChildcareDocumentTypeAlias),
        typeof(CouncilOfficeDocumentTypeAlias),
        typeof(DayCentreDocumentTypeAlias),
        typeof(LibraryDocumentTypeAlias),
        typeof(MobileLibraryStopDocumentTypeAlias),
        typeof(ParkDocumentTypeAlias),
        typeof(RecyclingSiteDocumentTypeAlias),
        typeof(RegistrationOfficeDocumentTypeAlias),
        typeof(SportLocationDocumentTypeAlias),
        typeof(CampaignLandingDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconHome, "", true, Description = "The starting page for browsing the entire site.")]
    public class HomePageDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTab("Content")]
        public HomePageContentTab Content { get; set; }
       
        [UmbracoTab("Latest", SortOrder = 1)]
        public LatestTab LatestTab { get; set; }

        [UmbracoTab("Social media and promotion", SortOrder = 2)]
        public SocialMediaAndPromotionTab SocialMedia { get; set; }

        [UmbracoProperty("Description", "pageDescription", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 1, mandatory: true)]
        public string Description { get; set; }

        [UmbracoProperty("Author notes", "phDefAuthorNotes", PropertyEditorAliases.RichTextPropertyEditor, dataTypeInstanceName: RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }

        [UmbracoProperty("Cache", "cache", BuiltInUmbracoDataTypes.DropDown, "Cache", sortOrder: 101, description: "Pages are cached for 24 hours by default.  If this page is particularly time-sensitive, pick a shorter time.")]
        public string Cache { get; set; }

        [UmbracoProperty("Copy of unpublished date (do not edit)", "unpublishAt", BuiltInUmbracoDataTypes.DateTime, sortOrder: 103)]
        public string UnpublishAt { get; set; }
    }
}
