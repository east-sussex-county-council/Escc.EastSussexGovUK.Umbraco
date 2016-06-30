using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan
{
    [UmbracoContentType("Council Plan Priorities page", "CouncilPlanPrioritiesPage", new[]
    {
        typeof(CouncilPlanTopicPageDocumentType)
    }, true, BuiltInUmbracoContentTypeIcons.IconOrderedList,
    Description = "Council Plan Priorities page.")]
    public class CouncilPlanPrioritiesPageDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public CouncilPlanPrioritiesPageContentTab Content { get; set; }
    }
}