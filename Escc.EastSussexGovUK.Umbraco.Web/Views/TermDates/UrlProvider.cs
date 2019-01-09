using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using System.Xml.XPath;
using Escc.Net;
using Escc.Net.Configuration;
using Exceptionless;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views.TermDates
{
    /// <summary>
    /// Downloads and caches school terms dates data from a URL. See <see cref="HttpRequestClient"/> for details of proxy support.
    /// </summary>
    public class UrlProvider : ITermDatesDataProvider
    {
        private readonly Uri _url;
        private readonly Cache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlProvider" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cache">The cache.</param>
        /// <exception cref="System.ArgumentNullException">url</exception>
        public UrlProvider(Uri url, Cache cache)
        {
            if (url == null) throw new ArgumentNullException("url");
            _url = url;
            _cache = cache;
        }

        /// <summary>
        /// Gets the term dates data as a <see cref="XPathDocument" />.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="WebException"></exception>
        public IXPathNavigable GetXPathData()
        {
            if (_cache != null && _cache["Escc.Schools.TermDates.Data"] != null)
            {
                return _cache["Escc.Schools.TermDates.Data"] as IXPathNavigable;
            }

            try
            {
                var client = new HttpRequestClient(new ConfigurationProxyProvider());
                var data = client.RequestXPath(_url);

                if (_cache != null)
                {
                    _cache.Insert("Escc.Schools.TermDates.Data", data, null, DateTime.UtcNow.AddDays(1), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
                }

                return data;
            }
            catch (WebException exception)
            {
                exception.Data.Add("URL requested", _url);
                exception.ToExceptionless().Submit();
                return new XDocument().CreateNavigator();
            }
        }
    }
}