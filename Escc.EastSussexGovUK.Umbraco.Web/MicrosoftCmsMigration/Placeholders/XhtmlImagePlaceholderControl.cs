using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders
{
    /// <summary>
    /// Substitute for Microsoft Content Management Server image placeholder control to enable migration of MCMS templates with minimal changes
    /// </summary>
    public class XhtmlImagePlaceholderControl : PlaceHolder
    {
        private string _placeholderToBind;

        /// <summary>
        /// Gets or sets the placeholder (Umbraco field) to bind.
        /// </summary>
        /// <value>
        /// The placeholder to bind.
        /// </value>
        public string PlaceholderToBind
        {
            get { return _placeholderToBind; }
            set
            {
                _placeholderToBind = value;
                LoadContentFromUmbraco();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this placeholder has content.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has content; otherwise, <c>false</c>.
        /// </value>
        public bool HasContent { get; set; }

        /// <summary>
        /// Gets or sets the image contained in the placeholder.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public Escc.Umbraco.PropertyTypes.Image Image { get; set; }

        /// <summary>
        /// Gets or sets the CSS classes to apply.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        public string CssClass { get; set; }

        /// <summary>
        /// Gets or sets the URL to link the image to.
        /// </summary>
        /// <value>
        /// The link URL.
        /// </value>
        public Uri LinkUrl { get; set; }

        /// <summary>
        /// Gets or sets the XHTML id for an associated image map
        /// </summary>
        public string AssociatedMapId { get; set; }

        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        /// <value>
        /// The tool tip.
        /// </value>
        [Obsolete("This was used for editing in Microsoft CMS")]
        public string ToolTip { get; set; }

        /// <summary>
        /// Loads the content from Umbraco in the same way MCMS would have loaded it from its node cache.
        /// </summary>
        private void LoadContentFromUmbraco()
        {
            if (String.IsNullOrWhiteSpace(PlaceholderToBind)) return;
            if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return;

            var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            if (content == null) return;

            var imageData = content.GetPropertyValue<IPublishedContent>(this.PlaceholderToBind + "_Content");
            if (imageData != null)
            {
                HasContent = true;
                Image = new Escc.Umbraco.PropertyTypes.Image()
                {
                    AlternativeText = imageData.Name,
                    ImageUrl = new Uri(imageData.Url, UriKind.Relative),
                    Width = imageData.GetPropertyValue<int>("umbracoWidth"),
                    Height = imageData.GetPropertyValue<int>("umbracoHeight")
                };
            }
        }

        /// <summary>
        /// Adds the HTML from the field, and raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            var tag = new StringBuilder();
            if (Image != null)
            {
                if (LinkUrl != null)
                {
                    tag.Append("<a href=\"").Append(LinkUrl).Append("\"");
                    if (!String.IsNullOrEmpty(CssClass)) tag.Append(" class=\"" + CssClass + "\"");
                    tag.Append(">");
                }

                tag.Append("<img src=\"").Append(Image.ImageUrl).Append("\" alt=\"").Append(Image.AlternativeText)
                    .Append("\" width=\"").Append(Image.Width).Append("\"");
                if (!String.IsNullOrEmpty(CssClass) && LinkUrl == null) tag.Append(" class=\"" + CssClass + "\"");

                // Add image map reference if set
                if (!string.IsNullOrEmpty(this.AssociatedMapId))
                {
                    tag.Append("usemap=\"#" + this.AssociatedMapId + "\"");
                }

                tag.Append(" />");

                if (LinkUrl != null)
                {
                    tag.Append("</a>");
                }

                Controls.Add(new LiteralControl(tag.ToString()));
            }

            base.OnPreRender(e);
        }

    }
}
