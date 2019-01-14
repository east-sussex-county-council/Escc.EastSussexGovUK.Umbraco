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
    /// Creates an Examine indexes of jobs posted to a TalentLink site, based on the TalentLink URLs in the 'TalentLinkPublicJobs...' configuration settings
    /// </summary>
    /// <seealso cref="BaseJobsIndexer" />
    public class PublicJobsIndexer : BaseJobsIndexer
    {
        private static readonly Uri ResultsUrl = new Uri(ConfigurationManager.AppSettings["TribePadPublicJobsResultsUrl"]);
        private static readonly Uri AdvertUrl = new Uri(ConfigurationManager.AppSettings["TribePadPublicJobsAdvertUrl"]);
        private static readonly Uri LookupValuesApiUrl = new Uri(ConfigurationManager.AppSettings["TribePadPublicJobsLookupValuesUrl"]);

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicJobsIndexer"/> class.
        /// </summary>
        public PublicJobsIndexer() : base(new JobsDataFromTribePad(ResultsUrl, AdvertUrl, 
            new TribePadJobParser(new JobsLookupValuesFromTribePad(LookupValuesApiUrl, new LookupValuesFromTribePadBuiltInFieldParser(), new LookupValuesFromTribePadCustomFieldParser(), new ConfigurationProxyProvider())),
            new TribePadJobParser(new JobsLookupValuesFromTribePad(LookupValuesApiUrl, new LookupValuesFromTribePadBuiltInFieldParser(), new LookupValuesFromTribePadCustomFieldParser(), new ConfigurationProxyProvider())), 
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
        public override BaseIndexProvider IndexProvider => ExamineManager.Instance.IndexProviderCollection["PublicJobsIndexer"];
    }
}