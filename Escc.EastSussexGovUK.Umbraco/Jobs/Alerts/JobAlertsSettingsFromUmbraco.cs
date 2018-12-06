using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Loads job alerts settings from Umbraco pages based on the 'Job alerts' document type
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.Alerts.IJobAlertSettingsProvider" />
    public class JobAlertsSettingsFromUmbraco : IJobAlertSettingsProvider
    {
        private readonly UmbracoHelper _umbraco;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobAlertsSettingsFromUmbraco"/> class.
        /// </summary>
        /// <param name="umbraco">The umbraco.</param>
        /// <exception cref="ArgumentNullException">umbraco</exception>
        public JobAlertsSettingsFromUmbraco(UmbracoHelper umbraco)
        {
            if (umbraco == null) throw new ArgumentNullException(nameof(umbraco));
            _umbraco = umbraco;
        }

        /// <summary>
        /// Gets the job alert settings for a <see cref="JobsSet" />
        /// </summary>
        /// <returns></returns>
        public JobAlertSettings GetJobAlertsSettings(JobsSet jobsSet)
        {
            JobAlertSettings settings = null;

            var jobAlertsSettingsPages = _umbraco.TypedContentAtXPath("//JobAlerts");
            foreach (var jobAlertSettingsPage in jobAlertsSettingsPages)
            {
                // Get the jobs set selected in the Umbraco page
                var selectedJobsSet = umbraco.library.GetPreValueAsString(jobAlertSettingsPage.GetPropertyValue<int>("PublicOrRedeployment_Content"));
                selectedJobsSet = Regex.Replace(selectedJobsSet.ToUpperInvariant(), "[^A-Z]", String.Empty);
                var jobsSetForPage = (JobsSet)Enum.Parse(typeof(JobsSet), selectedJobsSet, true);
                if (jobsSetForPage != jobsSet) continue;

                // Create settings from the page
                settings = new JobAlertSettings();

                settings.NewAlertEmailSubject = jobAlertSettingsPage.GetPropertyValue<string>("NewAlertEmailSubject_Alert_settings");
                settings.NewAlertEmailBodyHtml = jobAlertSettingsPage.GetPropertyValue<string>("NewAlertEmailBody_Alert_settings");
                settings.AlertEmailSubject = jobAlertSettingsPage.GetPropertyValue<string>("AlertEmailSubject_Alert_settings");
                settings.AlertEmailBodyHtml = jobAlertSettingsPage.GetPropertyValue<string>("AlertEmailBody_Alert_settings");

                var baseUrl = jobAlertSettingsPage.GetPropertyValue<string>("BaseUrl_Alert_settings");
                if (!String.IsNullOrEmpty(baseUrl))
                {
                    settings.ChangeAlertBaseUrl = new Uri(new Uri(baseUrl), jobAlertSettingsPage.Url());
                }
                else
                {
                    settings.ChangeAlertBaseUrl = new Uri(jobAlertSettingsPage.UrlAbsolute());
                }

                var jobAdvertPage = jobAlertSettingsPage.GetPropertyValue<IPublishedContent>("JobAdvertPage_Alert_settings");
                if (jobAdvertPage!= null)
                {
                    if (!String.IsNullOrEmpty(baseUrl))
                    {
                        settings.JobAdvertBaseUrl = new Uri(new Uri(baseUrl), jobAdvertPage.Url());
                    }
                    else
                    {
                        settings.JobAdvertBaseUrl = new Uri(jobAdvertPage.UrlAbsolute());
                    }
                }
            }

            return settings;
        }
    }
}