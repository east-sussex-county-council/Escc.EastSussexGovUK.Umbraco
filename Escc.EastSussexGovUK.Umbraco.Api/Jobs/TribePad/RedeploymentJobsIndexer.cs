using System;
using System.Configuration;
using System.Linq;
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
    /// Creates an Examine indexes of jobs posted to TribePad, based on the TribePad URLs in the 'TribePad...' configuration settings
    /// </summary>
    /// <seealso cref="BaseJobsIndexer" />
    public class RedeploymentJobsIndexer : BaseJobsIndexer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedeploymentJobsIndexer"/> class.
        /// </summary>
        public RedeploymentJobsIndexer() 
        {
            var resultsUrls = ConfigurationManager.AppSettings["TribePadRedeploymentJobsResultsUrls"]?.Split(',').Select(x => new Uri(x));
            var advertUrl = new Uri(ConfigurationManager.AppSettings["TribePadAdvertUrl"]);
            var lookupValuesApiUrl = new Uri(ConfigurationManager.AppSettings["TribePadLookupValuesUrl"]);
            var applyUrl = new Uri(ConfigurationManager.AppSettings["TribePadApplyUrl"]);

            // This setting is useful in a test environment where images are not present on the test domain
            var disableMediaDomainTransformer = ConfigurationManager.AppSettings["DoNotRemoveMediaDomainInJobAdverts"]?.ToUpperInvariant() == "TRUE";

            var proxyProvider = new ConfigurationProxyProvider();
            var lookupValuesProvider = new JobsLookupValuesFromTribePad(lookupValuesApiUrl, new LookupValuesFromTribePadBuiltInFieldParser(), new LookupValuesFromTribePadCustomFieldParser(), null, proxyProvider);
            var jobParser = new TribePadJobParser(lookupValuesProvider, new TribePadSalaryParser(lookupValuesProvider), new TribePadWorkPatternParser(lookupValuesProvider, new TribePadWorkPatternSplitter()), new TribePadLocationParser(), applyUrl);

            JobsProvider = new JobsDataFromTribePad(resultsUrls, advertUrl, jobParser, jobParser, proxyProvider);
            StopWordsRemover = new LuceneStopWordsRemover();
            TagSanitiser = new HtmlTagSanitiser();
            JobTransformers = new Dictionary<IEnumerable<IJobMatcher>, IEnumerable<IJobTransformer>>()
            {
                { new IJobMatcher[] { new JointCommunityRehabilitationMatcher(), new LocationMatcher("Lewes") }, new IJobTransformer[] { new SetJobLocationTransformer(new[] { "Crowborough", "Lewes", "Peacehaven",  "Wadhurst" }) } },
                { new IJobMatcher[] { new JointCommunityRehabilitationMatcher(), new LocationMatcher("Eastbourne") }, new IJobTransformer[] { new SetJobLocationTransformer(new[] { "Eastbourne", "Hailsham", "Polegate", "Seaford" }) } },
                { new IJobMatcher[] { new JointCommunityRehabilitationMatcher(), new LocationMatcher("Bexhill-on-Sea") }, new IJobTransformer[] { new SetJobLocationTransformer(new[] { "Bexhill-on-Sea", "Hastings", "Rural Rother" }) } },
                { new IJobMatcher[] { /* No matcher - apply to all jobs */ }, new IJobTransformer[] {
                    new HtmlAgilityPackFormatterAdapter(new IHtmlAgilityPackHtmlFormatter[] {
                        new RemoveUnwantedAttributesFormatter(new string[] { "style", "data-mce-style" }),
                        new RemoveUnwantedNodesFormatter(new[] { "font", "u", "comment()" }, false),
                        new RemoveUnwantedNodesFormatter(new[] { "style" }, true),
                        new RemoveElementsWithNoContentFormatter(new[] { "strong", "p" }),
                        new TruncateLongLinksFormatter(new HtmlLinkFormatter()),
                        new EmbeddedYouTubeVideosFormatter(),
                        new FakeListFormatter()
                    }),
                    new HtmlStringFormatterAdapter(new IHtmlStringFormatter[] {
                        new CloseEmptyElementsFormatter(),
                        new HouseStyleDateFormatter(),
                        disableMediaDomainTransformer ? null : new RemoveMediaDomainUrlTransformer()
                    })
                } }
            };
        }

        /// <summary>
        /// Gets the index provider.
        /// </summary>
        public override BaseIndexProvider IndexProvider => ExamineManager.Instance.IndexProviderCollection["RedeploymentJobsIndexer"];
    }
}