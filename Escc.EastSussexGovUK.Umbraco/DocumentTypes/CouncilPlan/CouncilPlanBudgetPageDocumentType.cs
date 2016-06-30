using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan
{
    [UmbracoContentType("Council Plan Budget page", "CouncilPlanBudgetPage", new[]
    {
        typeof(CouncilPlanBudgetPageDocumentType),
        typeof(CouncilPlanTopicPageDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconLegal,
    Description = "Council Plan budget page.")]
    public class CouncilPlanBudgetPageDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public CouncilPlanBudgetPageContentTab Content { get; set; }
    }
}