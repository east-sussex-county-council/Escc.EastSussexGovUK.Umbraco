using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Xml;
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
        private readonly IProxyProvider _proxyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlProvider" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cache">The cache.</param>
        /// <exception cref="System.ArgumentNullException">url</exception>
        public UrlProvider(Uri url, Cache cache, IProxyProvider proxyProvider)
        {
            if (url == null) throw new ArgumentNullException("url");
            _url = url;
            _cache = cache;
            _proxyProvider = proxyProvider;
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
                var data = RequestXPath(CreateRequest(_url, _proxyProvider));

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

        /// <summary>
        /// Creates a new <see cref="HttpWebRequest"/> for the specified URI, with proxy access configured.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        public HttpWebRequest CreateRequest(Uri requestUri, IProxyProvider proxyProvider)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            webRequest.UserAgent = "Escc.Net.HttpRequestClient"; // Some apps require a user-agent to be present
            if (proxyProvider != null)
            {
                IWebProxy proxy = proxyProvider.CreateProxy();
                if (proxy != null)
                {
                    webRequest.Proxy = proxy;
                    webRequest.Credentials = proxy.Credentials;
                }
                else
                {
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                }
            }
            else
            {
                webRequest.Credentials = CredentialCache.DefaultCredentials;
            }
            return webRequest;
        }

        /// <summary>
        /// Requests XML data from a URL and loads the response into an XPathDocument
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="System.Net.WebException"></exception>
        /// <exception cref="WebException">Thrown if XML response has an HTTP status other than 200 OK</exception>
        public IXPathNavigable RequestXPath(WebRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            using (var xmlResponse = request.GetResponse() as HttpWebResponse)
            {
                if (xmlResponse.StatusCode == HttpStatusCode.OK)
                {

                    var settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Ignore; // Allow a DTD to be present in the XML, since it generally is there in a web page
                    settings.XmlResolver = null; // But don't follow the link to the DTD, which generally leads to a timeout
                    var reader = XmlReader.Create(xmlResponse.GetResponseStream(), settings);
                    try
                    {
                        return new XPathDocument(reader);
                    }
                    catch (XmlException ex)
                    {
                        // try to make the request again, in order to include the XML that failed in the error object
                        using (var xmlResponseForError = CreateRequest(request.RequestUri, _proxyProvider).GetResponse() as HttpWebResponse)
                        {
                            if (xmlResponseForError.StatusCode == HttpStatusCode.OK)
                            {
                                using (var streamReader = new StreamReader(xmlResponseForError.GetResponseStream()))
                                {
                                    ex.Data.Add("XML", streamReader.ReadToEnd());
                                    throw;
                                }
                            }
                        }

                        // but if that doesn't work, at least include the remainder of the original stream
                        using (var streamReader = new StreamReader(xmlResponse.GetResponseStream()))
                        {
                            ex.Data.Add("XML", streamReader.ReadToEnd());
                            throw;
                        }
                    }
                }
                else
                {
                    throw new WebException(String.Format(CultureInfo.InvariantCulture, "Request for XML data received an HTTP response of {0} {1}", xmlResponse.StatusCode, xmlResponse.StatusDescription));
                }
            }
        }
    }
}