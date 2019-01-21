using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.Net;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// Loads jobs lookup values as HTML by making an HTTP request to a TribePad server
    /// </summary>
    public class JobsLookupValuesFromTribePad : IJobsLookupValuesProvider
    {
        private readonly Uri _lookupValuesApiUrl;
        private readonly IProxyProvider _proxy;
        private readonly IJobLookupValuesParser _builtInLookupValuesParser;
        private readonly IJobLookupValuesParser _customFieldLookupValuesParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromTribePad" /> class.
        /// </summary>
        /// <param name="lookupValuesApiUrl">The search URL.</param>
        /// <param name="builtInLookupValuesParser">The parser for lookup values in built-in fields in the TribePad XML.</param>
        /// <param name="customFieldLookupValuesParser">The parser for lookup values in custom fields in the TribePad XML.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <exception cref="System.ArgumentNullException">lookupValuesApiUrl</exception>
        /// <exception cref="System.ArgumentNullException">builtInLookupValuesParser</exception>
        /// <exception cref="System.ArgumentNullException">customFieldLookupValuesParser</exception>
        public JobsLookupValuesFromTribePad(Uri lookupValuesApiUrl, IJobLookupValuesParser builtInLookupValuesParser, IJobLookupValuesParser customFieldLookupValuesParser, IProxyProvider proxy)
        {
            if (lookupValuesApiUrl == null) throw new ArgumentNullException(nameof(lookupValuesApiUrl));
            if (builtInLookupValuesParser == null) throw new ArgumentNullException(nameof(builtInLookupValuesParser));
            if (customFieldLookupValuesParser == null) throw new ArgumentNullException(nameof(customFieldLookupValuesParser));

            _lookupValuesApiUrl = lookupValuesApiUrl;
            _builtInLookupValuesParser = builtInLookupValuesParser;
            _customFieldLookupValuesParser = customFieldLookupValuesParser;
            _proxy = proxy;
        }


        /// <summary>
        /// Reads the locations where jobs can be based
        /// </summary>
        /// <returns></returns>
        public Task<IList<JobsLookupValue>> ReadLocations()
        {
            return Task.FromResult((IList<JobsLookupValue>)new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Text = "Alfriston" },
                new JobsLookupValue() { Text = "Battle" },
                new JobsLookupValue() { Text = "Bewbush" },
                new JobsLookupValue() { Text = "Bexhill-on-Sea" },
                new JobsLookupValue() { Text = "Bognor Regis" },
                new JobsLookupValue() { Text = "Brighton" },
                new JobsLookupValue() { Text = "Burgess Hill" },
                new JobsLookupValue() { Text = "Chailey" },
                new JobsLookupValue() { Text = "Chichester" },
                new JobsLookupValue() { Text = "Chiddingly" },
                new JobsLookupValue() { Text = "Countywide" },
                new JobsLookupValue() { Text = "Crawley" },
                new JobsLookupValue() { Text = "Crowborough" },
                new JobsLookupValue() { Text = "Ditchling" },
                new JobsLookupValue() { Text = "Eastbourne" },
                new JobsLookupValue() { Text = "Etchingham" },
                new JobsLookupValue() { Text = "Exceat, Nr Seaford" },
                new JobsLookupValue() { Text = "Fletching" },
                new JobsLookupValue() { Text = "Flimwell" },
                new JobsLookupValue() { Text = "Ford" },
                new JobsLookupValue() { Text = "Forest Row" },
                new JobsLookupValue() { Text = "Frant" },
                new JobsLookupValue() { Text = "Groombridge" },
                new JobsLookupValue() { Text = "Hailsham" },
                new JobsLookupValue() { Text = "Hampden Park" },
                new JobsLookupValue() { Text = "Hartfield" },
                new JobsLookupValue() { Text = "Hassocks" },
                new JobsLookupValue() { Text = "Hastings" },
                new JobsLookupValue() { Text = "Haywards Heath" },
                new JobsLookupValue() { Text = "Heathfield" },
                new JobsLookupValue() { Text = "Hellingly" },
                new JobsLookupValue() { Text = "Henfield" },
                new JobsLookupValue() { Text = "Herstmonceux" },
                new JobsLookupValue() { Text = "Hollington" },
                new JobsLookupValue() { Text = "Horndean" },
                new JobsLookupValue() { Text = "Horsham" },
                new JobsLookupValue() { Text = "Hove" },
                new JobsLookupValue() { Text = "Kingston" },
                new JobsLookupValue() { Text = "Langney" },
                new JobsLookupValue() { Text = "Laughton" },
                new JobsLookupValue() { Text = "Lewes" },
                new JobsLookupValue() { Text = "Lewes and Kingston" },
                new JobsLookupValue() { Text = "Maresfield" },
                new JobsLookupValue() { Text = "Mayfield" },
                new JobsLookupValue() { Text = "Midhurst" },
                new JobsLookupValue() { Text = "Newhaven" },
                new JobsLookupValue() { Text = "Northiam" },
                new JobsLookupValue() { Text = "Nutley" },
                new JobsLookupValue() { Text = "Old Heathfield" },
                new JobsLookupValue() { Text = "Orbis office bases" },
                new JobsLookupValue() { Text = "Ore" },
                new JobsLookupValue() { Text = "Other" },
                new JobsLookupValue() { Text = "Peacehaven" },
                new JobsLookupValue() { Text = "Petersfield" },
                new JobsLookupValue() { Text = "Pevensey Bay" },
                new JobsLookupValue() { Text = "Polegate" },
                new JobsLookupValue() { Text = "Portslade" },
                new JobsLookupValue() { Text = "Portsmouth" },
                new JobsLookupValue() { Text = "Ringmer" },
                new JobsLookupValue() { Text = "Robertsbridge" },
                new JobsLookupValue() { Text = "Rotherfield" },
                new JobsLookupValue() { Text = "Rye" },
                new JobsLookupValue() { Text = "Saltdean" },
                new JobsLookupValue() { Text = "Seaford" },
                new JobsLookupValue() { Text = "Shoreham" },
                new JobsLookupValue() { Text = "Southwick" },
                new JobsLookupValue() { Text = "St Leonards-on-Sea" },
                new JobsLookupValue() { Text = "Stone Cross" },
                new JobsLookupValue() { Text = "Telscombe" },
                new JobsLookupValue() { Text = "Ticehurst" },
                new JobsLookupValue() { Text = "Tunbridge Wells" },
                new JobsLookupValue() { Text = "Uckfield" },
                new JobsLookupValue() { Text = "Wadhurst" },
                new JobsLookupValue() { Text = "Wealden" },
                new JobsLookupValue() { Text = "Willingdon" },
                new JobsLookupValue() { Text = "Worthing" }
            });
        }

        /// <summary>
        /// Reads the job types or categories
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadJobTypes()
        {
            return await ReadLookupValuesFromApi(_builtInLookupValuesParser, "job_categories").ConfigureAwait(false);
        }

        /// <summary>
        /// Reads the organisations advertising jobs
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadOrganisations()
        {
            return await ReadLookupValuesFromApi(_builtInLookupValuesParser, "business_units").ConfigureAwait(false);
        }

        /// <summary>
        /// Reads the salary ranges that jobs can be categorised as
        /// </summary>
        /// <returns></returns>
        public Task<IList<JobsLookupValue>> ReadSalaryRanges()
        {
            return Task.FromResult((IList<JobsLookupValue>)new List<JobsLookupValue>()
            {
                new JobsLookupValue() { Text = "£0 to £9,999" },
                new JobsLookupValue() { Text = "£10,000 to £14,999" }, 
                new JobsLookupValue() { Text = "£15,000 to £19,999" },
                new JobsLookupValue() { Text = "£20,000 to £24,999" },
                new JobsLookupValue() { Text = "£25,000 to £34,999" },
                new JobsLookupValue() { Text = "£35,000 to £49,999" },
                new JobsLookupValue() { Text = "£50,000 and over" }
            });
        }


        /// <summary>
        /// Reads the salary frequencies, eg hourly, weekly, annually
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadSalaryFrequencies()
        {
            return await ReadLookupValuesFromApi(_builtInLookupValuesParser, "salary_frequencies").ConfigureAwait(false);
        }

        /// <summary>
        /// Reads the work patterns, eg full time or part time
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadWorkPatterns()
        {
            var results = await ReadLookupValuesFromApi(_customFieldLookupValuesParser, "Working Pattern").ConfigureAwait(false);
            ConvertResultsToSentenceCase(results);
            return results;
        }

        private void ConvertResultsToSentenceCase(IList<JobsLookupValue> results)
        {
            for (var i = 0; i < results.Count; i++)
            {
                // By replacing initial caps with a lowercase letter only where they follow straight after another word,
                // we get values in sentence case that still have an initial cap after any punctuation.
                results[i].Text = Regex.Replace(results[i].Text, @"[a-z]\s[A-Z]", ToLower);
            }
        }

        /// <summary>
        /// MatchEvaluator for a regular expression in <see cref="ReadWorkPatterns"/>
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string ToLower(Match m)
        {
            return m.Value.ToLower(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Reads the contract types, eg fixed term or permanent
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadContractTypes()
        {
            var results = await ReadLookupValuesFromApi(_builtInLookupValuesParser, "job_types").ConfigureAwait(false);
            ConvertResultsToSentenceCase(results);
            return results;
        }

        private async Task<IList<JobsLookupValue>> ReadLookupValuesFromApi(IJobLookupValuesParser parser, string fieldName)
        {
            var htmlStream = await ReadXml(_lookupValuesApiUrl, _proxy).ConfigureAwait(false);

            using (var reader = new StreamReader(htmlStream))
            {
                return parser.ParseLookupValues(reader.ReadToEnd(), fieldName);
            }
        }

        /// <summary>
        /// Initiates an HTTP request and returns the XML.
        /// </summary>
        /// <returns></returns>
        private static async Task<Stream> ReadXml(Uri url, IProxyProvider proxy)
        {
            var handler = new HttpClientHandler()
            {
                Proxy = proxy?.CreateProxy()
            };
            var client = new HttpClient(handler);
            return await client.GetStreamAsync(url).ConfigureAwait(false);
        }
    }
}