using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;
using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Definition for the Umbraco 'Jobs component' document type
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.UmbracoGeneratedBase" />
    [UmbracoContentType("Jobs component", "JobsComponent", new Type[] {
        typeof(JobSearchResultsDocumentType),
        typeof(JobsComponentDocumentType),
        typeof(JobsRssDocumentType),
        typeof(ProblemJobsRssDocumentType),
        typeof(JobsSearchDocumentType),
        typeof(JobAdvertDocumentType),
        typeof(JobAlertsDocumentType)
    }, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconBrick, 
    Description = "A standard page hosting a component of the TalentLink application.")]
    public class JobsComponentDocumentType : CustomerFocusBaseDocumentType
    {
        [UmbracoTemplate(DisplayName = "Jobs CSS", Alias = "JobsCss")]
        public string JobsCss { get; set; }

        [UmbracoTab("Content")]
        public JobsComponentContentTab Content { get; set; }
    }
}