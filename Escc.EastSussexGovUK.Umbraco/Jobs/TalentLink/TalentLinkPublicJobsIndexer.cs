using System;
using System.Configuration;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// Creates an Examine indexes of jobs posted to a TalentLink site, based on the TalentLink URLs in the 'TalentLinkPublicJobs...' configuration settings
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.BaseJobsIndexer" />
    public class TalentLinkPublicJobsIndexer : BaseJobsIndexer
    {
        private static readonly Uri SearchUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkPublicJobsSearchUrl"]).LinkUrl;
        private static readonly Uri ResultsUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkPublicJobsResultsUrl"]).LinkUrl;
        private static readonly Uri AdvertUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkPublicJobsAdvertUrl"]).LinkUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="TalentLinkPublicJobsIndexer"/> class.
        /// </summary>
        public TalentLinkPublicJobsIndexer() : base(new JobsDataFromTalentLink(SearchUrl, ResultsUrl, AdvertUrl, new ConfigurationProxyProvider(), new JobLookupValuesHtmlParser(), new JobResultsHtmlParser(new TalentLinkSalaryParser()), new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser())), new StopWordsRemover())
        {
        }
    }
}