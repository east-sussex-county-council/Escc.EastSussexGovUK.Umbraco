using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// Loads jobs lookup values as HTML by making an HTTP request to a TalentLink server
    /// </summary>
    public class JobsLookupValuesFromTalentLink : IJobsLookupValuesProvider
    {
        private readonly Uri _searchUrl;
        private readonly IProxyProvider _proxy;
        private readonly IJobLookupValuesParser _lookupValuesParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsDataFromTalentLink" /> class.
        /// </summary>
        /// <param name="searchUrl">The search URL.</param>
        /// <param name="lookupValuesParser">The parser for lookup values in the TalentLink HTML.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <exception cref="System.ArgumentNullException">sourceUrl</exception>
        public JobsLookupValuesFromTalentLink(Uri searchUrl, IJobLookupValuesParser lookupValuesParser, IProxyProvider proxy)
        {
            if (searchUrl == null) throw new ArgumentNullException(nameof(searchUrl));
            if (lookupValuesParser == null) throw new ArgumentNullException(nameof(lookupValuesParser));

            _searchUrl = searchUrl;
            _lookupValuesParser = lookupValuesParser;
            _proxy = proxy;
        }


        /// <summary>
        /// Reads the locations where jobs can be based
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadLocations()
        {
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV39");
        }

        /// <summary>
        /// Reads the job types or categories
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadJobTypes()
        {
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV40");
        }

        /// <summary>
        /// Reads the organisations advertising jobs
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IList<JobsLookupValue>> ReadOrganisations()
        {
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV52");
        }

        /// <summary>
        /// Reads the salary ranges that jobs can be categorised as
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadSalaryRanges()
        {
            var salaryRanges = await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV46");
            foreach (var salaryRange in salaryRanges)
            {
                salaryRange.Text = salaryRange.Text.Replace(" to ", " - ");
            }
            return salaryRanges;
        }

        /// <summary>
        /// Reads the work patterns, eg full time or part time
        /// </summary>
        /// <returns></returns>
        public async Task<IList<JobsLookupValue>> ReadWorkPatterns()
        {
            return await ReadLookupValuesFromTalentLink(_lookupValuesParser, "LOV50");
        }

        private async Task<IList<JobsLookupValue>> ReadLookupValuesFromTalentLink(IJobLookupValuesParser parser, string fieldName)
        {
            var htmlStream = await ReadHtml(_searchUrl, _proxy);

            using (var reader = new StreamReader(htmlStream))
            {
                return parser.ParseLookupValues(reader.ReadToEnd(), fieldName);
            }
        }

        /// <summary>
        /// Initiates an HTTP request and returns the HTML.
        /// </summary>
        /// <returns></returns>
        private static async Task<Stream> ReadHtml(Uri url, IProxyProvider proxy)
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