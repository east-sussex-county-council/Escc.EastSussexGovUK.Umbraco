using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
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
            _umbraco = umbraco ?? throw new ArgumentNullException(nameof(umbraco));
        }

        /// <summary>
        /// Gets the job alert settings for each <see cref="JobsSet" />
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<JobsSet, JobAlertSettings> GetJobAlertsSettings()
        {
            var settings = new Dictionary<JobsSet, JobAlertSettings>();

            var jobAlertsSettingsPages = _umbraco.TypedContentAtXPath("//JobAlerts");
            foreach (var jobAlertSettingsPage in jobAlertsSettingsPages)
            {
                // Get the jobs set selected in the Umbraco page
                var selectedJobsSet = umbraco.library.GetPreValueAsString(jobAlertSettingsPage.GetPropertyValue<int>("PublicOrRedeployment_Content"));
                selectedJobsSet = Regex.Replace(selectedJobsSet.ToUpperInvariant(), "[^A-Z]", String.Empty);
                var jobsSet = (JobsSet)Enum.Parse(typeof(JobsSet), selectedJobsSet);
                if (settings.ContainsKey(jobsSet)) continue;

                // Create a jobs set and populate the settings from the page
                settings.Add(jobsSet, new JobAlertSettings());

                settings[jobsSet].NewAlertEmailSubject = jobAlertSettingsPage.GetPropertyValue<string>("NewAlertEmailSubject_Content");
                settings[jobsSet].NewAlertEmailBodyHtml = jobAlertSettingsPage.GetPropertyValue<string>("NewAlertEmailBody_Content");
                settings[jobsSet].AlertEmailSubject = jobAlertSettingsPage.GetPropertyValue<string>("AlertEmailSubject_Content");
                settings[jobsSet].AlertEmailBodyHtml = jobAlertSettingsPage.GetPropertyValue<string>("AlertEmailBody_Content");
                settings[jobsSet].ChangeAlertBaseUrl = new Uri(jobAlertSettingsPage.UrlAbsolute());
            }

            var jobAdvertPages = _umbraco.TypedContentAtXPath("//JobAdvert");
            foreach (var jobAdvert in jobAdvertPages)
            {
                var selectedJobsSet = umbraco.library.GetPreValueAsString(jobAdvert.GetPropertyValue<int>("PublicOrRedeployment_Content"));
                selectedJobsSet = Regex.Replace(selectedJobsSet.ToUpperInvariant(), "[^A-Z]", String.Empty);
                var jobsSet = (JobsSet)Enum.Parse(typeof(JobsSet), selectedJobsSet);
                if (!settings.ContainsKey(jobsSet)) continue;

                settings[jobsSet].JobAdvertBaseUrl = new Uri(jobAdvert.UrlAbsolute());
            }

            return settings;
        }
    }
}