using System;
using System.Configuration;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Jobs.Examine;
using Escc.Html;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// Creates an Examine indexes of jobs posted to a TalentLink site, based on the TalentLink URLs in the 'TalentLinkPublicJobs...' configuration settings
    /// </summary>
    /// <seealso cref="BaseJobsIndexer" />
    public class PublicJobsIndexer : BaseJobsIndexer
    {
        private static readonly Uri ResultsUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkPublicJobsResultsUrl"]).LinkUrl;
        private static readonly Uri AdvertUrl = new TalentLinkUrl(ConfigurationManager.AppSettings["TalentLinkPublicJobsAdvertUrl"]).LinkUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicJobsIndexer"/> class.
        /// </summary>
        public PublicJobsIndexer() : base(new JobsDataFromTalentLink(ResultsUrl, AdvertUrl, 
            new TalentLinkJobResultsHtmlParser(new TalentLinkSalaryParser()), 
            new TalentLinkJobAdvertHtmlParser(new TalentLinkSalaryParser(), new TalentLinkWorkPatternParser()), 
            new ConfigurationProxyProvider(), true), 
            new LuceneStopWordsRemover(), 
            new HtmlTagSantiser())
        {
        }
    }
}