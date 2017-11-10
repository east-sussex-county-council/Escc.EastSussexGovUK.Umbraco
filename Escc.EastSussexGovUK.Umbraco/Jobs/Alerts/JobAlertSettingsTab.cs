using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using System;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Alerts
{
    /// <summary>
    /// Alert settings tab for <see cref="JobAlertsDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class JobAlertSettingsTab : TabBase
    {
        [UmbracoProperty("Base URL", "BaseUrl", UrlDataType.PropertyEditorAlias, UrlDataType.DataTypeName, sortOrder: 1,
            Description = "Base URL for links to jobs and job alert settings, if not the current domain")]
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the job advert page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the advert page
        /// </value>
        [UmbracoProperty("Job advert page", "JobAdvertPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 7,
            Description = "Select the page used to display a single job advert, based on the 'Job advert' document type")]
        public string JobAdvertPage { get; set; }

        [UmbracoProperty("Alert confirmation email: subject", "NewAlertEmailSubject", BuiltInUmbracoDataTypes.Textbox, sortOrder: 9, 
            Description = "The subject line of the email sent out when someone creates an alert. If this is blank, no email will be sent.")]
        public string NewAlertEmailSubject { get; set; }

        [UmbracoProperty("Alert confirmation email: body", "NewAlertEmailBody", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 10,
            Description = "The body of the email sent out when someone creates an alert. Use {alert-description} to insert the search the alert is based on. Link to {change-alert-url} to insert a link to change or cancel the alert.")]
        public string NewAlertEmailBody { get; set; }

        [UmbracoProperty("Alert email: subject", "AlertEmailSubject", BuiltInUmbracoDataTypes.Textbox, sortOrder: 11, mandatory: true,
            Description = "The subject line of job alert emails.")]
        public string AlertEmailSubject { get; set; }

        [UmbracoProperty("Alert email: body", "AlertEmailBody", PropertyEditorAliases.RichTextPropertyEditor, RichTextEsccWithFormattingDataType.DataTypeName, sortOrder: 12,
            Description = "The body of job alert emails. Use {jobs} to insert the jobs.")]
        public string AlertEmailBody { get; set; }
    }
}