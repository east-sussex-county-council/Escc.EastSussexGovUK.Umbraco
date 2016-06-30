using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan
{
    [UmbracoContentType("Council Plan Home page", "CouncilPlanHomePage", new[]
    {
        typeof(CouncilPlanBudgetPageDocumentType),
        typeof(CouncilPlanPrioritiesPageDocumentType),
        typeof(CouncilPlanTopicPageDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconBriefcase,
   Description = "Council Plan home page.")]
    public class CouncilPlanHomePageDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public CouncilPlanHomePageContentTab Content { get; set; }
    }
}