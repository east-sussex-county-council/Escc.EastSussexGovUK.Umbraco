using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    public class JobAlertSentTableEntity : TableEntity
    {
        /// <summary>
        /// Gets the set of jobs which this alert was sent for
        /// </summary>
        public string JobsSet { get; set; }
    }
}