using System;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration.Placeholders
{
    public class XhtmlAttachmentPlaceholderControl : SingleAttachmentPlaceholderControl
    {
        private string[] allowedExtensions;

        /// <summary>
        /// Gets or sets a comma-separated list of file extensions which this placeholder will accept
        /// </summary>
        public string AllowedExtensions
        {
            get
            {
                return String.Join(",", this.allowedExtensions);
            }
            set
            {
                this.allowedExtensions = String.IsNullOrEmpty(value) ? new string[0] : value.Split(',');
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has content.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has content; otherwise, <c>false</c>.
        /// </value>
        public bool HasContent
        {
            get { return this.AttachmentUrl != null; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override void OnPreRender(EventArgs e)
        {
            if (Resource != null && Resource.Values.ContainsKey("umbracoExtension"))
            {
                string extension = Resource.Values["umbracoExtension"].ToLowerInvariant();

                if (allowedExtensions != null)
                {
                    var allowed = new List<string>(allowedExtensions).Contains(extension);
                    if (!allowed)
                    {
                        this.Visible = false;
                    }
                }
            }
            base.OnPreRender(e);
        }
    }
}