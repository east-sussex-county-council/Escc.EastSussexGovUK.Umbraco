using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exceptionless;
using log4net;
using log4net.Config;

namespace Escc.Jobs.UpdateIndexes
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            ExceptionlessClient.Default.Startup();
            XmlConfigurator.Configure();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var apiBaseUrl = new Uri(ConfigurationManager.AppSettings["JobsApiBaseUrl"]);

                var apiUser = ConfigurationManager.AppSettings["ApiUser"];
                if (string.IsNullOrEmpty(apiUser))
                {
                    throw new ConfigurationErrorsException("appSettings > ApiUser was not set in app.config");
                }

                var apiPassword = ConfigurationManager.AppSettings["ApiPassword"];
                if (string.IsNullOrEmpty(apiPassword))
                {
                    throw new ConfigurationErrorsException("appSettings > ApiPassword was not set in app.config");
                }

                var request = WebRequest.Create(new Uri($"{apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/JobsIndexerApi/UpdatePublicJobs/"));
                request.Credentials = new NetworkCredential(apiUser, apiPassword);
                request.Timeout = Timeout.Infinite;
                _log.Info($"Requesting {request.RequestUri}");
                using (var response = request.GetResponse())
                {
                }

                request = WebRequest.Create(new Uri($"{apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/JobsIndexerApi/UpdateRedeploymentJobs/"));
                request.Credentials = new NetworkCredential(apiUser, apiPassword);
                request.Timeout = Timeout.Infinite;
                _log.Info($"Requesting {request.RequestUri}");
                using (var response = request.GetResponse())
                {
                }

                request = WebRequest.Create(new Uri($"{apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/JobsIndexerApi/ReplacePublicJobsLookupValues/"));
                request.Credentials = new NetworkCredential(apiUser, apiPassword);
                request.Timeout = Timeout.Infinite;
                _log.Info($"Requesting {request.RequestUri}");
                using (var response = request.GetResponse())
                {
                }

                request = WebRequest.Create(new Uri($"{apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/JobsIndexerApi/ReplaceRedeploymentJobsLookupValues/"));
                request.Credentials = new NetworkCredential(apiUser, apiPassword);
                request.Timeout = Timeout.Infinite;
                _log.Info($"Requesting {request.RequestUri}");
                using (var response = request.GetResponse())
                {
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                _log.Error(ex.Message);

                // it's important to rethrow so that it reports failure when run as an Azure WebJob
                throw;
            }
        }
    }
}
