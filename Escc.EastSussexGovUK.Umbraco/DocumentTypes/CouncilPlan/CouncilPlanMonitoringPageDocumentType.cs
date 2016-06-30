using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan
{
    [UmbracoContentType("Council Plan Monitoring page", "CouncilPlanMonitoringPage", null, true, BuiltInUmbracoContentTypeIcons.IconDiagnostics,
   Description = "Council Plan monitoring page.")]
    public class CouncilPlanMonitoringPageDocumentType : LegacyBaseDocumentType
    {
        [UmbracoTab("Content")]
        public CouncilPlanMonitoringPageContentTab Content { get; set; }
    }
}