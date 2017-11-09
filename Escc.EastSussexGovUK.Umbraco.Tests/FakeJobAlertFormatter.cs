using Escc.EastSussexGovUK.Umbraco.Jobs.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    internal class FakeJobAlertFormatter : IJobAlertFormatter
    {
        public string FormatAlert(IList<JobAlert> alerts)
        {
            return String.Empty;
        }

        public string FormatNewAlertConfirmation(JobAlert alert)
        {
            throw new NotImplementedException();
        }
    }
}
