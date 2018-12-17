using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Sync;

namespace Escc.EastSussexGovUK.Umbraco.Web.AzureConfiguration
{
    /// <summary>
    /// Registers a load-balanced Umbraco server as a slave server
    /// </summary>
    /// <remarks>Code from https://our.umbraco.org/documentation/Getting-Started/Setup/Server-Setup/load-balancing/flexible-advanced</remarks>
    /// <seealso cref="Umbraco.Core.Sync.IServerRegistrar2" />
    public class FrontEndReadOnlyServerRegistrar : IServerRegistrar2
    {
        public IEnumerable<IServerAddress> Registrations
        {
            get { return Enumerable.Empty<IServerAddress>(); }
        }
        public ServerRole GetCurrentServerRole()
        {
            return ServerRole.Slave;
        }
        public string GetCurrentServerUmbracoApplicationUrl()
        {
            return null;
        }
    }
}