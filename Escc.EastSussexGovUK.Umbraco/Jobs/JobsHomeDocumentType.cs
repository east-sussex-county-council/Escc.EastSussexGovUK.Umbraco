using System;
using Escc.EastSussexGovUK.Umbraco.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Guide;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task;
using Escc.EastSussexGovUK.Umbraco.Location;
using Umbraco.Inception.Attributes;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using Escc.EastSussexGovUK.Umbraco.Forms;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Definition for the 'Jobs' Umbraco document type
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase.CustomerFocusBaseDocumentType" />
    [UmbracoContentType("Jobs", "JobsHome", new Type[]
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
        typeof(CouncilOfficeDocumentType),
        typeof(CampaignLandingDocumentType),
        typeof(PersonDocumentType),
        typeof(JobsHomeDocumentType),
        typeof(JobSearchResultsDocumentType),
        typeof(JobsComponentDocumentType),
        typeof(JobsRssDocumentType),
        typeof(ProblemJobsRssDocumentType),
        typeof(JobsSearchDocumentType),
        typeof(JobAdvertDocumentType),
        typeof(JobAlertsDocumentType),
        typeof(FormDocumentType)
    }, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconReception, 
    Description = "The start page for the jobs section of the website.")]
    public class JobsHomeDocumentType : CustomerFocusBaseDocumentType
    {
        [UmbracoTemplate(DisplayName = "Jobs CSS", Alias = "JobsCss")]
        public string JobsCss { get; set; }

        [UmbracoTab("Content")]
        public JobsHomeContentTab Content { get; set; }
    }
}