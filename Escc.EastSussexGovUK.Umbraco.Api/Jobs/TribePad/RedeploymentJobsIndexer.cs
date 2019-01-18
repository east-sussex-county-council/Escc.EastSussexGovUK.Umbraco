
using System;
using System.Configuration;
using Escc.Html;
using Examine.Providers;
using Examine;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine;
using Escc.EastSussexGovUK.Umbraco.Examine;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.JobTransformers;
using Escc.Net.Configuration;
using Escc.EastSussexGovUK.Umbraco.UrlTransformers;
using Escc.EastSussexGovUK.Umbraco.Api.Jobs.HtmlFormatters;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// Creates an Examine indexes of jobs posted to a TalentLink site, based on the TalentLink URLs in the 'TalentLinkRedeploymentJobs...' configuration settings
    /// </summary>
    /// <seealso cref="BaseJobsIndexer" />
    public class RedeploymentJobsIndexer : BaseJobsIndexer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedeploymentJobsIndexer"/> class.
        /// </summary>
        public RedeploymentJobsIndexer() 
        {
            var resultsUrl = new Uri(ConfigurationManager.AppSettings["TribePadRedeploymentJobsResultsUrl"]);
            var advertUrl = new Uri(ConfigurationManager.AppSettings["TribePadRedeploymentJobsAdvertUrl"]);
            var lookupValuesApiUrl = new Uri(ConfigurationManager.AppSettings["TribePadRedeploymentJobsLookupValuesUrl"]);

            var proxyProvider = new ConfigurationProxyProvider();
            var lookupValuesProvider = new JobsLookupValuesFromTribePad(lookupValuesApiUrl, new LookupValuesFromTribePadBuiltInFieldParser(), new LookupValuesFromTribePadCustomFieldParser(), proxyProvider);
            var jobParser = new TribePadJobParser(lookupValuesProvider, new TribePadSalaryParser(lookupValuesProvider));

            InitialiseDependencies(
                new JobsDataFromTribePad(resultsUrl, advertUrl, jobParser, jobParser, proxyProvider, true),
                new LuceneStopWordsRemover(),
                new HtmlTagSanitiser(),
                new Dictionary<IEnumerable<IJobMatcher>, IEnumerable<IJobTransformer>>()
                {
                    { new IJobMatcher[] { new JointCommunityRehabilitationMatcher(), new LocationMatcher("Lewes") }, new IJobTransformer[] { new SetJobLocationTransformer(new[] { "Crowborough", "Lewes", "Peacehaven",  "Wadhurst" }) } },
                    { new IJobMatcher[] { new JointCommunityRehabilitationMatcher(), new LocationMatcher("Eastbourne") }, new IJobTransformer[] { new SetJobLocationTransformer(new[] { "Eastbourne", "Hailsham", "Polegate", "Seaford" }) } },
                    { new IJobMatcher[] { new JointCommunityRehabilitationMatcher(), new LocationMatcher("Bexhill-on-Sea") }, new IJobTransformer[] { new SetJobLocationTransformer(new[] { "Bexhill-on-Sea", "Hastings", "Rural Rother" }) } },
                    { new IJobMatcher[] { /* No matcher - apply to all jobs */ }, new IJobTransformer[] {
                        new HtmlAgilityPackFormatterAdapter(new IHtmlAgilityPackHtmlFormatter[] {
                            new RemoveUnwantedAttributesFormatter(new string[] { "style" }),
                            new RemoveUnwantedElementsFormatter(new[] { "u" }, false),
                            new RemoveUnwantedElementsFormatter(new[] { "style" }, true),
                            new RemoveElementsWithNoContentFormatter(new[] { "strong", "p" }),
                            new TruncateLongLinksFormatter(new HtmlLinkFormatter()),
                            new EmbeddedYouTubeVideosFormatter()
                        }),
                        new HtmlStringFormatterAdapter(new IHtmlStringFormatter[] {
                            new CloseEmptyElementsFormatter(),
                            new HouseStyleDateFormatter(),
                            new RemoveMediaDomainUrlTransformer()
                        })
                    } }
                }
            );
        }

        /// <summary>
        /// Gets the index provider.
        /// </summary>
        public override BaseIndexProvider IndexProvider => ExamineManager.Instance.IndexProviderCollection["RedeploymentJobsIndexer"];
    }
}