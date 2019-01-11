using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        private readonly IJobLookupValuesParser _lookupValuesParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromTribePad" /> class.
        /// </summary>
        /// <param name="lookupValuesApiUrl">The search URL.</param>
        /// <param name="lookupValuesParser">The parser for lookup values in the TribePad HTML.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <exception cref="System.ArgumentNullException">sourceUrl</exception>
        public JobsLookupValuesFromTribePad(Uri lookupValuesApiUrl, IJobLookupValuesParser lookupValuesParser, IProxyProvider proxy)
        {
            if (lookupValuesApiUrl == null) throw new ArgumentNullException(nameof(lookupValuesApiUrl));
            if (lookupValuesParser == null) throw new ArgumentNullException(nameof(lookupValuesParser));

            _lookupValuesApiUrl = lookupValuesApiUrl;
            _lookupValuesParser = lookupValuesParser;
            _proxy = proxy;
        }


        /// <summary>
        /// Reads the locations where jobs can be based
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadLocations()
        {
            return new List<JobsLookupValue>();
        }

        /// <summary>
        /// Reads the job types or categories
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadJobTypes()
        {
            return await ReadLookupValuesFromApi(_lookupValuesParser, "job_categories");
        }

        /// <summary>
        /// Reads the organisations advertising jobs
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IList<JobsLookupValue>> ReadOrganisations()
        {
            return await ReadLookupValuesFromApi(_lookupValuesParser, "business_units");
        }

        /// <summary>
        /// Reads the salary ranges that jobs can be categorised as
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadSalaryRanges()
        {
            return new List<JobsLookupValue>();
        }

        /// <summary>
        /// Reads the work patterns, eg full time or part time
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadWorkPatterns()
        {
            return new List<JobsLookupValue>();
        }

        private async Task<IList<JobsLookupValue>> ReadLookupValuesFromApi(IJobLookupValuesParser parser, string fieldName)
        {
            var htmlStream = await ReadXml(_lookupValuesApiUrl, _proxy);

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
            return await client.GetStreamAsync(url);
        }
    }
}