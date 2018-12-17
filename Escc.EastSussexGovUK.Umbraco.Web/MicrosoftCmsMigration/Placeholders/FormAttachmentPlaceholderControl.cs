using System;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders
{
    /// <summary>
    /// Attachment placeholder for a form uploaded to a form download page
    /// </summary>
    [ValidationProperty("AttachmentUrl")]
    public class FormAttachmentPlaceholderControl : SingleAttachmentPlaceholderControl
    {
        private FormAttachmentType attachmentType;
        private ResourceManager resManager;

        /// <summary>
        /// Gets or sets whether this placeholder is for a PDF, an RTF, or an RTF which requires a signature
        /// </summary>
        public FormAttachmentType AttachmentType
        {
            get
            {
                return this.attachmentType;
            }
            set
            {
                this.attachmentType = value;
            }
        }

        /// <summary>
        /// Attachment placeholder for a form uploaded to a form download page
        /// </summary>
        public FormAttachmentPlaceholderControl()
        {
            this.resManager = (ResourceManager)HttpContext.Current.Cache.Get("FormPlaceholderControls");
            if (this.resManager == null)
            {
                this.resManager = PlaceholderResources.ResourceManager;
                HttpContext.Current.Cache.Insert("FormPlaceholderControls", this.resManager, null, DateTime.MaxValue, TimeSpan.FromMinutes(10));
            }
        }


        /// <summary>
        /// Present as a link, followed by a span with format and size info
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (this.AttachmentUrl == null)
            {
                this.Visible = false;
                return;
            }

            if (Resource != null && Resource.Values.ContainsKey("umbracoExtension"))
            {
                if (!AttachmentType.ToString().ToUpperInvariant().StartsWith(Resource.Values["umbracoExtension"].ToUpperInvariant(), StringComparison.Ordinal))
                {
                    this.Visible = false;
                    return;
                }
            }

            using (var presentationContainer = new HtmlGenericControl("span"))
            {
                if (!String.IsNullOrEmpty(CssClass)) presentationContainer.Attributes["class"] = CssClass;
                this.Controls.Add(presentationContainer);

                // AttachmentText property holds an string identifier for a .NET RESX resource
                StringBuilder resKey = new StringBuilder("FormAttachment");
                resKey.Append(this.AttachmentType);

                using (HtmlAnchor link = new HtmlAnchor())
                {
                    link.HRef = this.AttachmentUrl.ToString();
                    link.InnerText = this.resManager.GetString(resKey.ToString());
                    link.Attributes["class"] = "no-meta";

                    resKey.Append("Title");
                    StringBuilder linkTitle = new StringBuilder(this.resManager.GetString(resKey.ToString()));
                    if (linkTitle != null && linkTitle.Length > 0) link.Title = linkTitle.Append(" (").Append(this.resManager.GetString("Format" + this.attachmentType)).Append(")").ToString();
                    presentationContainer.Controls.Add(link);
                }

                // display the file size in brackets
                string size = "";
                if (Resource != null) size = CmsUtilities.GetResourceFileSize(Resource);
                if (size.Length > 0)
                {
                    using (HtmlGenericControl sizeElement = new HtmlGenericControl("span"))
                    {
                        sizeElement.InnerHtml = new StringBuilder().Append(" (").Append(this.resManager.GetString("Format" + this.attachmentType.ToString())).Append(" &#8211; <span class=\"downloadSize\">").Append(size).Append("</span>)").ToString();
                        sizeElement.Attributes.Add("class", "downloadDetail");
                        presentationContainer.Controls.Add(sizeElement);
                    }
                }

                this.Visible = true;
            }
        }

        /// <summary>
        /// The type of document expected in this placeholder
        /// </summary>
        public enum FormAttachmentType
        {
            /// <summary>
            /// PDF document to be printed and sent by post
            /// </summary>
            Pdf,

            /// <summary>
            /// RTF document to be filled in an emailed back
            /// </summary>
            Rtf,

            /// <summary>
            /// RTF document to be printed, signed and posted
            /// </summary>
            RtfAndSign,

            /// <summary>
            /// Excel file to print and post
            /// </summary>
            XlsPrint,

            /// <summary>
            /// Excel file to return by email
            /// </summary>
            Xls
        }


    }
}
