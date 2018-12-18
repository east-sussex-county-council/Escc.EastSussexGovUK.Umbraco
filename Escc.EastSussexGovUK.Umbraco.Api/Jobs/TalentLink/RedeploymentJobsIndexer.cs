﻿
using System;
using System.Configuration;
using Escc.Html;
using Escc.Net;
using Examine.Providers;
using Examine;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink
{
    /// <summary>
    /// Creates an Examine indexes of jobs posted to a TalentLink site, based on the TalentLink URLs in the 'TalentLinkRedeploymentJobs...' configuration settings
    /// </summary>
    /// <seealso cref="BaseJobsIndexer" />
    public class RedeploymentJobsIndexer : BaseJobsIndexer
    {
        private static readonly Uri ResultsUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkRedeploymentJobsResultsUrl"]).LinkUrl;
        private static readonly Uri AdvertUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkRedeploymentJobsAdvertUrl"]).LinkUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedeploymentJobsIndexer"/> class.
        /// </summary>
        public RedeploymentJobsIndexer() : base(new JobsDataFromTalentLink(ResultsUrl, AdvertUrl, 
            new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryParser()), 
            new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser()), 
            new ConfigurationProxyProvider(), true),
            new LuceneStopWordsRemover(),
            new HtmlTagSantiser(),
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