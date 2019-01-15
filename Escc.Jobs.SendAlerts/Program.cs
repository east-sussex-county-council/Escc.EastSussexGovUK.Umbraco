using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exceptionless;
using log4net;
using log4net.Config;

namespace Escc.Jobs.SendAlerts
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            ExceptionlessClient.Default.Startup();
            XmlConfigurator.Configure();

            try
            {
                if (!CheckEnvironmentPrecondition())
                {
                    return;
                }


                int? frequency = null;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Frequency"]))
                {
                    int parsedFrequency;
                    if (int.TryParse(ConfigurationManager.AppSettings["Frequency"], out parsedFrequency))
                    {
                        frequency = parsedFrequency;
                    }
                }

                bool forceResend = false;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ForceResend"]))
                {
                    bool parsedForceResend;
                    if (bool.TryParse(ConfigurationManager.AppSettings["ForceResend"], out parsedForceResend))
                    {
                        forceResend = parsedForceResend;
                    }
                }
                
                new JobAlertSender(log).SendAlerts(frequency, forceResend).RunSynchronously();
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                log.Error(ex.Message);

                // it's important to rethrow so that it reports failure when run as an Azure WebJob
                throw;
            }
        }

        private static bool CheckEnvironmentPrecondition()
        {
            var precondition = ConfigurationManager.AppSettings["Precondition"];
            if (!string.IsNullOrEmpty(precondition))
            {
                var split = ConfigurationManager.AppSettings["Precondition"].Split('=');
                if (split.Length == 2)
                {
                    var result = (Environment.GetEnvironmentVariable(split[0]).Equals(split[1], StringComparison.OrdinalIgnoreCase));
                    log.Info("Precondition " + precondition + (result ? " OK." : " failed."));
                    return result;
                }
            }
            return true;
        }
    }
}
