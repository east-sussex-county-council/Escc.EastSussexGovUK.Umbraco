using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan
{
    [UmbracoContentType("Council Plan Topic page", "CouncilPlanTopicPage", new[]
    {
        typeof(CouncilPlanTopicPageDocumentType),
        typeof(CouncilPlanMonitoringPageDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconArticle,
   Description = "Council Plan topic page.")]
    public class CouncilPlanTopicPageDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public CouncilPlanTopicPageContentTab Content { get; set; }
    }
}