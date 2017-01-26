
using System;
using System.Configuration;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Escc.Html;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
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
            new ConfigurationProxyProvider()), 
            new LuceneStopWordsRemover(),
            new HtmlTagSantiser())
        {
        }
    }
}