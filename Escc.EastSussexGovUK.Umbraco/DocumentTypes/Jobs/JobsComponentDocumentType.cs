using System;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Jobs
{
    [UmbracoContentType("Jobs component", "JobsComponent", new Type[] {}, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconBrick, 
    Description = "A standard page hosting a component of the TalentLink application.")]
    public class JobsComponentDocumentType : UmbracoGeneratedBase
    {
        [UmbracoTab("Content")]
        public JobsComponentContentTab Content { get; set; }
    }
}