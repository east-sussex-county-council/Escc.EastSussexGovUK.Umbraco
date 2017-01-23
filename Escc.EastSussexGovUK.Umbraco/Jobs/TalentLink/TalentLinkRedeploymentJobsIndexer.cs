using System;
using System.Configuration;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// Creates an Examine indexes of jobs posted to a TalentLink site, based on the TalentLink URLs in the 'TalentLinkRedeploymentJobs...' configuration settings
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.BaseJobsIndexer" />
    public class TalentLinkRedeploymentJobsIndexer : BaseJobsIndexer
    {
        private static readonly Uri SearchUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkRedeploymentJobsSearchUrl"]).LinkUrl;
        private static readonly Uri ResultsUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkRedeploymentJobsResultsUrl"]).LinkUrl;
        private static readonly Uri AdvertUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkRedeploymentJobsAdvertUrl"]).LinkUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="TalentLinkRedeploymentJobsIndexer"/> class.
        /// </summary>
        public TalentLinkRedeploymentJobsIndexer() : base(new JobsDataFromTalentLink(SearchUrl, ResultsUrl, AdvertUrl, new ConfigurationProxyProvider(), new JobLookupValuesHtmlParser(), new JobResultsHtmlParser(new TalentLinkSalaryParser()), new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser())), new StopWordsRemover())
        {
        }
    }
}