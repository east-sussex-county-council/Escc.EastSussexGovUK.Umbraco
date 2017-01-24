using System;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Content tab for <see cref="JobsRssDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class JobsRssContentTab : TabBase
    {
        /// <summary>
        /// Gets or sets the job detail page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the detail page
        /// </value>
        [UmbracoProperty("Job advert page", "JobDetailPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 6,
            Description = "Select the job detail page used to display a single job advert, based on the 'Job advert' document type")]
        public string JobDetailPage { get; set; }

        [UmbracoProperty("Which jobs should this page show?", "PublicOrRedeployment", PublicOrRedeploymentDataType.PropertyEditor, PublicOrRedeploymentDataType.DataTypeName, sortOrder: 7, mandatory: true)]
        public string PublicOrRedeploymentJobs { get; set; }
    }
}