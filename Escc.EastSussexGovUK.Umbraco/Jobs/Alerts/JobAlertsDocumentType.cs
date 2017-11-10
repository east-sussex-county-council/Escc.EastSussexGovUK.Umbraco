using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Umbraco.Inception.Attributes;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Definition for the Umbraco 'Jobs component' document type
    /// </summary>
    /// <seealso cref="UmbracoGeneratedBase" />
    [UmbracoContentType("Job alerts", "JobAlerts", null, true, allowAtRoot: false, icon: BuiltInUmbracoContentTypeIcons.IconRadioAlt, 
    Description = "Configure the job alerts feature which allows users to subscribe to alerts about new jobs matching their search.")]
    public class JobAlertsDocumentType : CustomerFocusBaseDocumentType
    {
        [UmbracoTemplate(DisplayName = "Jobs CSS", Alias = "JobsCss")]
        public string JobsCss { get; set; }

        [UmbracoTab("Content", 0)]
        public JobAlertsContentTab Content { get; set; }

        [UmbracoTab("Alert settings", 0)]
        public JobAlertSettingsTab AlertSettings { get; set; }
    }
}