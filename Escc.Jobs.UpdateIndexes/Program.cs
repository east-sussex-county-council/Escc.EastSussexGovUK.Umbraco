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

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var apiBaseUrl = new Uri("https://" + Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["HostnameEnvironmentVariable"]));

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

            // Note: if these requests return 500 Internal Server Error running on Azure it may be because the external IP address of the 
            // Azure App Service where this is running needs to be allowed in the IP restrictions for the API site
            Exception exceptionToThrow = null;
            try
            {
                CallApi(new Uri($"{apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/JobsIndexerApi/UpdatePublicJobs/"), apiUser, apiPassword);
            }
            catch (Exception ex)
            {
                // Report the exception but save throwing it for later - for now, continue to try the subsequent API calls
                ex.ToExceptionless().Submit();
                _log.Error(ex.Message);
                exceptionToThrow = ex;
            }

            try
            {
                CallApi(new Uri($"{apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/JobsIndexerApi/UpdateRedeploymentJobs/"), apiUser, apiPassword);
            }
            catch (Exception ex)
            {
                // Report the exception but save throwing it for later - for now, continue to try the subsequent API calls
                ex.ToExceptionless().Submit();
                _log.Error(ex.Message);
                if (exceptionToThrow == null) exceptionToThrow = ex;
            }

            try
            {
                CallApi(new Uri($"{apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/JobsIndexerApi/ReplacePublicJobsLookupValues/"), apiUser, apiPassword);
            }
            catch (Exception ex)
            {
                // Report the exception but save throwing it for later - for now, continue to try the subsequent API calls
                ex.ToExceptionless().Submit();
                _log.Error(ex.Message);
                if (exceptionToThrow == null) exceptionToThrow = ex;
            }

            try
            {
                CallApi(new Uri($"{apiBaseUrl.ToString().TrimEnd('/')}/umbraco/api/JobsIndexerApi/ReplaceRedeploymentJobsLookupValues/"), apiUser, apiPassword);
            }
            catch (Exception ex)
            {
                // Report the exception but save throwing it for later - for now, continue to try the subsequent API calls
                ex.ToExceptionless().Submit();
                _log.Error(ex.Message);
                if (exceptionToThrow == null) exceptionToThrow = ex;
            }

            if (exceptionToThrow != null)
            {
                // it's important to rethrow so that it reports failure when run as an Azure WebJob
                throw exceptionToThrow;
            }
        }

        private static void CallApi(Uri apiMethodUrl, string apiUser, string apiPassword)
        {
            var request = WebRequest.Create(apiMethodUrl);
            request.Credentials = new NetworkCredential(apiUser, apiPassword);
            request.Timeout = Timeout.Infinite;
            _log.Info($"Requesting {request.RequestUri}");
            using (var response = request.GetResponse())
            {
            }
        }
    }
}
