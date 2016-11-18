using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Escc.Net;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Loads TalentLink HTML by making an HTTP request to a TalentLink server
    /// </summary>
    public class TalentLinkHtmlFromHttpRequest
    {
        private readonly Uri _sourceUrl;
        private readonly IProxyProvider _proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="TalentLinkHtmlFromHttpRequest"/> class.
        /// </summary>
        /// <param name="sourceUrl">The source URL.</param>
        /// <param name="proxy">The proxy (optional).</param>
        /// <exception cref="System.ArgumentNullException">sourceUrl</exception>
        public TalentLinkHtmlFromHttpRequest(Uri sourceUrl, IProxyProvider proxy)
        {
            if (sourceUrl == null) throw new ArgumentNullException(nameof(sourceUrl));
            _sourceUrl = sourceUrl;
            _proxy = proxy;
        }

        /// <summary>
        /// Initiates an HTTP request and returns the HTML.
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> ReadHtml()
        {
            var handler = new HttpClientHandler()
            {
                Proxy = _proxy?.CreateProxy()
            };
            var client = new HttpClient(handler);
            return await client.GetStreamAsync(_sourceUrl);
        }
    }
}