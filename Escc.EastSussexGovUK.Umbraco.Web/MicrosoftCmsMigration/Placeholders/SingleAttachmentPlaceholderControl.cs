using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.Umbraco.Media;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders
{
    /// <summary>
    /// Substitute for Microsoft Content Management Server plain text placeholder control to enable migration of MCMS templates with minimal changes
    /// </summary>
    public class SingleAttachmentPlaceholderControl : PlaceHolder
    {
        private string _placeholderToBind;
        private MediaValues _value;

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
        /// Gets the URL of the attached file
        /// </summary>
        public Uri AttachmentUrl {
            get
            {
                if (_value != null && _value.Values.ContainsKey("umbracoFile"))
                {
                    return new Uri(_value.Values["umbracoFile"], UriKind.RelativeOrAbsolute);
                } 
                return null;
            } 
        }


        [Obsolete("This property is implied by setting ElementName")]
        public bool RenderContainerElement { get; set; }
        
        /// <summary>
        /// Gets or sets the HTML element to use when rendering a container element
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// Gets or sets the CSS classes to apply, if <see cref="ElementName"/> is set.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        public string CssClass { get; set; }

        /// <summary>
        /// Gets the media item.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        protected MediaValues Resource { get { return _value; } }

        /// <summary>
        /// Loads the content from Umbraco in the same way MCMS would have loaded it from its node cache.
        /// </summary>
        private void LoadContentFromUmbraco()
        {
            if (String.IsNullOrWhiteSpace(this.PlaceholderToBind)) return;
            if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return;

            var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            if (content == null) return;

            // Code from http://shazwazza.com/post/ultra-fast-media-performance-in-umbraco/
            
            // Unfortunately this hits the database, but the usual approach of content.GetPropertyValue<IPublishedContent>(property alias) 
            // doesn't return the extra Description field. The next recommendation is using Examine but we know that doesn't work reliably on Azure.
            var mediaPicker = content.Properties.Single(prop => prop.PropertyTypeAlias == this.PlaceholderToBind + "_Content");
            if (mediaPicker != null && !String.IsNullOrEmpty(mediaPicker.DataValue.ToString()))
            {
                var media = umbraco.library.GetMedia(Int32.Parse(mediaPicker.DataValue.ToString(), CultureInfo.InvariantCulture), false);
                if (media != null && media.Current != null)
                {
                    media.MoveNext();
                    _value = new MediaValues(media.Current);
                }
            }
        }

        /// <summary>
        /// Adds a link to the media item, and raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override void OnPreRender(EventArgs e)
        {
            if (_value != null)
            {
                var htmlToRender = new StringBuilder();

                if (!String.IsNullOrEmpty(this.ElementName))
                {
                    htmlToRender.Append("<").Append(ElementName.ToLowerInvariant());
                    if (!String.IsNullOrEmpty(ID)) htmlToRender.Append(" id=\"").Append(ID).Append("\"");
                    if (!String.IsNullOrEmpty(CssClass)) htmlToRender.Append(" class=\"").Append(CssClass).Append("\"");
                    htmlToRender.Append(">");
                }

                htmlToRender.Append("<a href=\"").Append(AttachmentUrl).Append("\">").Append(_value.Name).Append("</a>");

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
