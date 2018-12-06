using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Escc.Jobs.SendAlerts
{
    /// <summary>
    /// Command line arguments for the programme
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Sends job alerts which are configured for a particular frequency (in days), or any frequency if blank
        /// </summary>
        [Option("frequency", Required = false)]
        public int? Frequency { get; set; }

        /// <summary>
        /// If set to <c>true</c> force resend of alerts already sent (for testing).
        /// </summary>
        [Option("forceResend", Required = false)]
        public bool ForceResend { get; set; }
    }
}
