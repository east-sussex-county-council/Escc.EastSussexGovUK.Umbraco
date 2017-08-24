using Exceptionless;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Sync;

namespace Escc.EastSussexGovUK.Umbraco.AzureConfiguration
{
    /// <summary>
    /// Explicitly registers back office servers as master and front-end servers as slave in a load-balanced setup
    /// </summary>
    /// <remarks>Implements the recommendation on https://our.umbraco.org/documentation/Getting-Started/Setup/Server-Setup/load-balancing/flexible-advanced</remarks>
    /// <seealso cref="Umbraco.Core.ApplicationEventHandler" />
    public class RegisterServerRoleEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            try
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["IsUmbracoBackOffice"]))
                {
                    ServerRegistrarResolver.Current.SetServerRegistrar(new FrontEndReadOnlyServerRegistrar());
                }
                else
                {
                    ServerRegistrarResolver.Current.SetServerRegistrar(new MasterServerRegistrar());
                }
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
            }
        }
    }
}