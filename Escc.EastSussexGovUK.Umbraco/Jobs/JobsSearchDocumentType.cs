using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Definition for the Umbraco 'Jobs component' document type
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.UmbracoGeneratedBase" />
    [UmbracoContentType("Jobs search", "JobsSearch", new Type[] {typeof(JobsRssDocumentType), typeof(ProblemJobsRssDocumentType), typeof(JobSearchResultsDocumentType), typeof(JobsComponentDocumentType),
        typeof(JobAdvertDocumentType)}, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconSearch, 
    Description = "The search page for the jobs section of the website.")]
    public class JobsSearchDocumentType : CustomerFocusBaseDocumentType
    {
        [UmbracoTemplate(DisplayName = "Jobs CSS", Alias = "JobsCss")]
        public string JobsCss { get; set; }

        [UmbracoTab("Content")]
        public JobsSearchContentTab Content { get; set; }
    }
}