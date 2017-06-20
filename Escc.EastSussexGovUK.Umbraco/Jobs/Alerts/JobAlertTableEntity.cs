using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    public class JobAlertTableEntity : TableEntity
    {
        public string Criteria { get; set; }
        public int Frequency { get; set; }
    }
}