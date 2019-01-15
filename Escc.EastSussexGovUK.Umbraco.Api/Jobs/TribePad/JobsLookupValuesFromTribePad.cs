﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.Net;

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
            return Task.FromResult((IList<JobsLookupValue>)new List<JobsLookupValue>());
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
            for (var i = 0; i<results.Count;i++)
            {
                results[i].Text = results[i].Text.Humanize(LetterCasing.Sentence);
            }
            return results;
        }

        /// <summary>
        /// Reads the contract types, eg fixed term or permanent
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadContractTypes()
        {
            return await ReadLookupValuesFromApi(_builtInLookupValuesParser, "job_types").ConfigureAwait(false);
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