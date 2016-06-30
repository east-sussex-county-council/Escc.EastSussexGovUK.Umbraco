using System;
using System.Resources;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration.Placeholders
{
    /// <summary>
    /// Placeholder for the URL of an XHTML form, which links to the form using standard link text
    /// </summary>
    public class FormUrlPlaceholderControl : TextPlaceholderControl
    {
        private HtmlAnchor link;
        private ResourceManager resManager;

        /// <summary>
        /// Gets or sets the resource manager key for the link text
        /// </summary>
        public string LinkText { get; set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="placeholder">The placeholder.</param>
        /// <returns></returns>
        public static string GetValue(Placeholder placeholder)
        {
            if (placeholder == null) throw new ArgumentNullException("placeholder");
            return (placeholder.Value == null) ? String.Empty : placeholder.Value.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUrlPlaceholderControl"/> class.
        /// </summary>
        public FormUrlPlaceholderControl()
        {
            this.LinkText = "FormAttachmentXhtml";
            this.resManager = (ResourceManager)HttpContext.Current.Cache.Get("FormPlaceholderControls");
            if (this.resManager == null)
            {
                this.resManager = PlaceholderResources.ResourceManager;
                HttpContext.Current.Cache.Insert("FormPlaceholderControls", this.resManager, null, DateTime.MaxValue, TimeSpan.FromMinutes(10));
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.Text)) return;

            using (var presentationContainer = new HtmlGenericControl("span"))
            {
                if (!String.IsNullOrEmpty(CssClass)) presentationContainer.Attributes["class"] = CssClass;
                this.Controls.Add(presentationContainer);

                this.link = new HtmlAnchor();
                this.link.InnerText = this.resManager.GetString(this.LinkText);
                this.link.HRef = HttpUtility.HtmlEncode(this.Text.Trim());
                presentationContainer.Controls.Add(this.link);
                presentationContainer.Visible = (this.link.HRef.Length > 0);
            }
        }
    }
}