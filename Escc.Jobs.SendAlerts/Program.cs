using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
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
                Parser.Default.ParseArguments<Options>(args)
                       .WithParsed<Options>(o =>
                       {
                           new JobAlertSender(log).SendAlerts(o.Frequency, o.ForceResend);
                       });
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                log.Error(ex.Message);

                // it's important to rethrow so that it reports failure when run as an Azure WebJob
                throw;
            }
        }


    }
}
