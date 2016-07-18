using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.Guide
{
    [UmbracoContentType("Guide step", "GuideStep", new Type[0], true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconFootprints,
        Description = "One of a series of pages in a specific order, which tell people all they need to know about a subject.")]
    public class GuideStepDocumentType : CustomerFocusBaseDocumentType
    {
        [UmbracoTab("Content")]
        public GuideStepContentTab Content { get; set; }
    }
}