using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration.Placeholders
{
    /// <summary>
    /// Substitute for Microsoft Content Management Server plain text placeholder control to enable migration of MCMS templates with minimal changes
    /// </summary>
    public class TextPlaceholderControl : PlaceHolder
    {
        private string _placeholderToBind;
        private string _value;

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
        /// Gets or sets the CSS classes to apply.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        public string CssClass { get; set; }

        /// <summary>
        /// Gets or sets the HTML element to use when rendering a container element
        /// </summary>
        public string ElementName { get; set; }

        [Obsolete("This property is implied by setting ElementName")]
        public bool RenderContainerElement { get; set; }

        /// <summary>
        /// Gets the text in the placeholder
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        protected string Text
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// Loads the content from Umbraco in the same way MCMS would have loaded it from its node cache.
        /// </summary>
        private void LoadContentFromUmbraco()
        {
            if (String.IsNullOrWhiteSpace(this.PlaceholderToBind)) return;
            if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return;

            var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            if (content == null) return;

            _value = content.GetPropertyValue<string>(this.PlaceholderToBind + "_Content");

            HasContent = !String.IsNullOrWhiteSpace(_value);
        }

        /// <summary>
        /// Adds the HTML from the field, and raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override void OnPreRender(EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(_value))
            {
                var htmlToRender = new StringBuilder();

                if (!String.IsNullOrEmpty(this.ElementName))
                {
                    htmlToRender.Append("<").Append(ElementName.ToLowerInvariant());

                    // GS Fix: Should be testing for "Not" empty
                    if (!String.IsNullOrEmpty(ID)) htmlToRender.Append(" id=\"").Append(ID).Append("\"");
                    if (!String.IsNullOrEmpty(CssClass)) htmlToRender.Append(" class=\"").Append(CssClass).Append("\"");
                    htmlToRender.Append(">");
                }

                htmlToRender.Append(_value);

                if (!String.IsNullOrEmpty(this.ElementName))
                {
                    htmlToRender.Append("</").Append(ElementName.ToLowerInvariant()).Append(">");
                }

                Controls.Add(new LiteralControl(htmlToRender.ToString()));
            }

            base.OnPreRender(e);
        }

    }
}
