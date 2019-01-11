
using System;
using System.Configuration;
using Escc.Html;
using Escc.Net;
using Examine.Providers;
using Examine;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers;
using Escc.Net.Configuration;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// Creates an Examine indexes of jobs posted to a TalentLink site, based on the TalentLink URLs in the 'TalentLinkRedeploymentJobs...' configuration settings
    /// </summary>
    /// <seealso cref="BaseJobsIndexer" />
    public class RedeploymentJobsIndexer : BaseJobsIndexer
    {
        private static readonly Uri ResultsUrl = new Uri(ConfigurationManager.AppSettings["TribePadRedeploymentJobsResultsUrl"]);
        private static readonly Uri AdvertUrl = new Uri(ConfigurationManager.AppSettings["TribePadRedeploymentJobsAdvertUrl"]);

        /// <summary>
        /// Initializes a new instance of the <see cref="RedeploymentJobsIndexer"/> class.
        /// </summary>
        public RedeploymentJobsIndexer() : base(new JobsDataFromTribePad(ResultsUrl, AdvertUrl, 
            new TribePadJobParser(),
            new TribePadJobParser(),
            new ConfigurationProxyProvider(), true),
            new LuceneStopWordsRemover(),
            new HtmlTagSanitiser(),
            new Dictionary<IEnumerable<IJobMatcher>, IEnumerable<IJobTransformer>>()
            {
                { new IJobMatcher[] { new JointCommunityRehabilitationMatcher(), new LocationMatcher("Lewes") }, new IJobTransformer[] { new SetJobLocationTransformer(new[] { "Crowborough", "Lewes", "Peacehaven",  "Wadhurst" }) } },
                { new IJobMatcher[] { new JointCommunityRehabilitationMatcher(), new LocationMatcher("Eastbourne") }, new IJobTransformer[] { new SetJobLocationTransformer(new[] { "Eastbourne", "Hailsham", "Polegate", "Seaford" }) } },
                { new IJobMatcher[] { new JointCommunityRehabilitationMatcher(), new LocationMatcher("Bexhill-on-Sea") }, new IJobTransformer[] { new SetJobLocationTransformer(new[] { "Bexhill-on-Sea", "Hastings", "Rural Rother" }) } }
            })
        {
        }

        /// <summary>
        /// Gets the index provider.
        /// </summary>
        public override BaseIndexProvider IndexProvider => ExamineManager.Instance.IndexProviderCollection["RedeploymentJobsIndexer"];
    }
}