using System;
using System.Net.Http.Headers;
using System.Web.Http;
using Exceptionless;
using Umbraco.Core;
using System.Net.Http.Formatting;

namespace Escc.EastSussexGovUK.Umbraco.WebApi
{
    /// <summary>
    /// Configure the default behaviours of Web APIs
    /// </summary>
    public class WebApiConfig : ApplicationEventHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiConfig"/> class.
        /// </summary>
        public WebApiConfig()
        {
            try
            {
                // When a browser requests the API, return JSON by default
                GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

                // Tell JSON.net how to serialise HTML strings, used by the Web API for jobs
                GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IHtmlStringConverter());

                // Enable CORS support for Web APIs, where the [CorsPolicyFromConfig] attribute is applied
                GlobalConfiguration.Configuration.EnableCors();
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }
        }
    }
}