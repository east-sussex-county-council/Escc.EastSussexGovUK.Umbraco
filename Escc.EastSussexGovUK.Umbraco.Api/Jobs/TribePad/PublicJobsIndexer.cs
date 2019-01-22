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
    /// Creates an Examine indexes of jobs posted to a TalentLink site, based on the TalentLink URLs in the 'TalentLinkPublicJobs...' configuration settings
    /// </summary>
    /// <seealso cref="BaseJobsIndexer" />
    public class PublicJobsIndexer : BaseJobsIndexer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicJobsIndexer"/> class.
        /// </summary>
        public PublicJobsIndexer() 
        {
            var resultsUrl = new Uri(ConfigurationManager.AppSettings["TribePadPublicJobsResultsUrl"]);
            var advertUrl = new Uri(ConfigurationManager.AppSettings["TribePadPublicJobsAdvertUrl"]);
            var lookupValuesApiUrl = new Uri(ConfigurationManager.AppSettings["TribePadPublicJobsLookupValuesUrl"]);
            var applyUrl = new Uri(ConfigurationManager.AppSettings["TribePadApplyUrl"]);

            // This setting is useful in a test environment where images are not present on the test domain
            var disableMediaDomainTransformer = ConfigurationManager.AppSettings["DoNotRemoveMediaDomainInJobAdverts"]?.ToUpperInvariant() == "TRUE";

            var proxyProvider = new ConfigurationProxyProvider();
            var lookupValuesProvider = new JobsLookupValuesFromTribePad(lookupValuesApiUrl, new LookupValuesFromTribePadBuiltInFieldParser(), new LookupValuesFromTribePadCustomFieldParser(), proxyProvider);
            var jobParser = new TribePadJobParser(lookupValuesProvider, new TribePadSalaryParser(lookupValuesProvider), new TribePadWorkPatternParser(lookupValuesProvider), applyUrl);

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
                            new RemoveUnwantedNodesFormatter(new[] { "u", "comment()" }, false),
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
                }
            );
        }

        /// <summary>
        /// Gets the index provider.
        /// </summary>
        public override BaseIndexProvider IndexProvider => ExamineManager.Instance.IndexProviderCollection["PublicJobsIndexer"];
    }
}