using System;
using System.Configuration;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Escc.Net;
using Examine;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// Creates an Examine indexes of lookup values jobs posted to a TalentLink site, based on the TalentLink URLs in the 'TalentLinkRedeploymentJobs...' configuration settings
    /// </summary>
    /// <seealso cref="BaseLookupValuesIndexer" />
    public class RedeploymentJobsLookupValuesIndexer : BaseLookupValuesIndexer
    {
        private static readonly Uri SearchUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkRedeploymentJobsSearchUrl"]).LinkUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedeploymentJobsLookupValuesIndexer"/> class.
        /// </summary>
        public RedeploymentJobsLookupValuesIndexer() : base(new JobsLookupValuesFromTalentLink(SearchUrl, new TalentLinkLookupValuesHtmlParser(), new ConfigurationProxyProvider()))
        {
        }

        /// <summary>
        /// Gets the jobs data provider used to check how many jobs match each lookup value
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This cannot be injected into the constructor as, when the provider uses Examine, it is too early in the lifecycle and crashes IIS!
        /// </remarks>
        protected override IJobsDataProvider GetJobsDataProvider()
        {
            return new JobsDataFromExamine(ExamineManager.Instance.SearchProviderCollection["RedeploymentJobsSearcher"], null, null);
        }
    }
}