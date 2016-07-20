using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.UmbracoDocumentTypes;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates
{
    [UmbracoContentType("Campaign content page", "CampaignContent", new Type[]
    {
        typeof(LandingDocumentTypeAlias), 
        typeof(LocationDocumentTypeAlias),
        typeof(TaskDocumentTypeAlias), 
        typeof(PersonDocumentType),
        typeof(LandingPageWithPicturesDocumentTypeAlias),
        typeof(GuideDocumentTypeAlias),
        typeof(StandardLandingPageDocumentTypeAlias),
        typeof(StandardTopicPageDocumentTypeAlias),
        typeof(StandardDownloadPageDocumentTypeAlias),
        typeof(MapDocumentTypeAlias),
        typeof(FormDownloadPageDocumentTypeAlias),
        typeof(RecyclingSiteDocumentTypeAlias),
        typeof(LibraryDocumentTypeAlias),
        typeof(MobileLibraryStopDocumentTypeAlias),
        typeof(ChildcareDocumentTypeAlias),
        typeof(CouncilOfficeDocumentTypeAlias),
        typeof(SportLocationDocumentTypeAlias),
        typeof(ParkDocumentTypeAlias),
        typeof(RegistrationOfficeDocumentTypeAlias),
        typeof(DayCentreDocumentTypeAlias),
        typeof(CampaignLandingDocumentType),
        typeof(CampaignContentDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconParachuteDrop, 
    Description="A content page for a marketing campaign")]
    public class CampaignContentDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTemplate(DisplayName = "Campaign content page CSS", Alias = "CampaignContentCss")]
        public string CampaignContentCss { get; set; }

        [UmbracoTab("Content", 0)]
        public EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates.CampaignContentContentTab ContentTab { get; set; }

        [UmbracoTab("Design", 1)]
        public EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates.CampaignContentDesignTab DesignTab { get; set; }

        [UmbracoProperty("Page URL", "umbracoUrlName", BuiltInUmbracoDataTypes.Textbox, sortOrder: 0)]
        public Uri UmbracoUrlName { get; set; }

        [UmbracoProperty("Description", "pageDescription", BuiltInUmbracoDataTypes.TextboxMultiple, sortOrder: 1, mandatory: true)]
        public string Description { get; set; }

        [UmbracoProperty("Author notes", "authorNotes", PropertyEditorAliases.RichTextPropertyEditor, RichTextAuthorNotesDataType.DataTypeName, sortOrder: 2)]
        public string AuthorNotes { get; set; }

        [UmbracoProperty("Cache", "cache", BuiltInUmbracoDataTypes.DropDown, "Cache", sortOrder: 101, description: "Pages are cached for 24 hours by default.  If this page is particularly time-sensitive, pick shorter time.")]
        public string Cache { get; set; }

        [UmbracoProperty("Copy of unpublished date (do not edit)", "unpublishAt", BuiltInUmbracoDataTypes.DateTime, sortOrder: 103)]
        public string UnpublishAt { get; set; }
    }
}

