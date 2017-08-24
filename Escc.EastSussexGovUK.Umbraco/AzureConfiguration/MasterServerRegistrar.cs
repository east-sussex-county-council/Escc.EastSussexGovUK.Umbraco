using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Sync;

namespace Escc.EastSussexGovUK.Umbraco.AzureConfiguration
{
    /// <summary>
    /// Registers a load-balanced Umbraco server as the master server
    /// </summary>
    /// <remarks>Code from https://our.umbraco.org/documentation/Getting-Started/Setup/Server-Setup/load-balancing/flexible-advanced</remarks>
    /// <seealso cref="Umbraco.Core.Sync.IServerRegistrar2" />
    public class MasterServerRegistrar : IServerRegistrar2
    {
        public IEnumerable<IServerAddress> Registrations
        {
            get { return Enumerable.Empty<IServerAddress>(); }
        }
        public ServerRole GetCurrentServerRole()
        {
            return ServerRole.Master;
        }
        public string GetCurrentServerUmbracoApplicationUrl()
        {
            //NOTE: If you want to explicitly define the URL that your application is running on,
            // this wil be used for the server to communicate with itself, you can return the 
            // custom path here and it needs to be in this format:
            // http://www.mysite.com/umbraco

            return null;
        }
    }
}