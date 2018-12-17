using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.AzureConfiguration
{
    /// <summary>
    /// Returns a partial machine name in an HTTP header if the site is requested from a known IP, to aid in debugging when load-balanced on cloud hosting
    /// </summary>
    /// <seealso cref="System.Web.IHttpModule" />
    public class MachineNameModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(ReturnMachineNameHeader);
        }

        #endregion

        public void ReturnMachineNameHeader(Object source, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["IpAddressesForDebugInfo"]))
            {
                var ips = new List<string>(ConfigurationManager.AppSettings["IpAddressesForDebugInfo"].Split(','));
                if (ips.Contains(HttpContext.Current.Request.UserHostAddress))
                {
                    // Only return a partial machine name, so that the information is only useful if the actual full machine names are already known
                    HttpContext.Current.Response.Headers.Add("X-ESCC-Machine", Environment.MachineName.Substring(Environment.MachineName.Length - 3));
                }
            }
        }
    }
}
