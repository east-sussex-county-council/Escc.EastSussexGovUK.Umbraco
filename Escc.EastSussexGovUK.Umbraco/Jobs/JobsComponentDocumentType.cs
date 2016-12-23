using System;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Definition for the Umbraco 'Jobs component' document type
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.UmbracoGeneratedBase" />
    [UmbracoContentType("Jobs component", "JobsComponent", new Type[] {typeof(JobsRssDocumentType)}, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconBrick, 
    Description = "A standard page hosting a component of the TalentLink application.")]
    public class JobsComponentDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTemplate(DisplayName = "Jobs CSS", Alias = "JobsCss")]
        public string JobsCss { get; set; }

        [UmbracoTab("Content")]
        public JobsComponentContentTab Content { get; set; }
    }
}