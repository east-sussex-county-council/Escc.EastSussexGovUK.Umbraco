﻿using System;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.Umbraco.PropertyEditors;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Umbraco.Inception.Attributes;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    /// <summary>
    /// Content tab for <see cref="JobAdvertDocumentType"/>
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.TabBase" />
    public class JobAdvertContentTab : TabBase
    {
        /// <summary>
        /// Gets or sets the jobs service logo
        /// </summary>
        /// <value>
        /// The jobs logo.
        /// </value>
        [UmbracoProperty("Logo", "JobsLogo", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 1, Description = "Select the logo for the jobs service")]
        public string JobsLogo { get; set; }


        /// <summary>
        /// Gets or sets the jobs start page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the home page
        /// </value>
        [UmbracoProperty("Jobs home page", "JobsHomePage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 2, Description = "Select the jobs home page, to be linked from the logo")]
        public string JobsHomePage { get; set; }
        
        /// <summary>
        /// Gets or sets the background image for the header
        /// </summary>
        /// <value>
        /// The background image
        /// </value>
        [UmbracoProperty("Header background image", "HeaderBackgroundImage", BuiltInUmbracoDataTypes.MediaPicker, sortOrder: 3, Description = "Select the background image for the page header")]
        public string HeaderBackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the login page
        /// </summary>
        /// <value>
        /// A reference to the Umbraco node for the login page
        /// </value>
        [UmbracoProperty("Login page", "LoginPage", BuiltInUmbracoDataTypes.ContentPickerAlias, sortOrder: 4, Description = "Select the jobs login page, based on the 'Jobs component' document type")]
        public string LoginPage { get; set; }

        [UmbracoProperty("Which jobs should this page show?", "PublicOrRedeployment", PublicOrRedeploymentDataType.PropertyEditor, PublicOrRedeploymentDataType.DataTypeName, sortOrder: 7, mandatory: true)]
        public string PublicOrRedeploymentJobs { get; set; }

    }
}