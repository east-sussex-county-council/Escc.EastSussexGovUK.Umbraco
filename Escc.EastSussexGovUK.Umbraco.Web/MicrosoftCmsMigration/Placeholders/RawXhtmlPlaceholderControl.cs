using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders
{
    /// <summary>
    /// Substitute for Microsoft Content Management Server raw XHTML placeholder control to enable migration of MCMS templates with minimal changes
    /// </summary>
    public class RawXhtmlPlaceholderControl : PlaceHolder
    {
        private string _placeholderToBind;
        private string _value;

        /// <summary>
        /// Gets the default XML that went into an empty placeholder in Microsoft CMS to prevent parsing errors.
        /// </summary>
        /// <value>
        /// The default XML.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string DefaultXml
        {
            get { return "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<DefaultXHTML />"; }
        }


        /// <summary>
        /// Gets or sets the edit mode hint text.
        /// </summary>
        /// <value>
        /// The hint text.
        /// </value>
        [Obsolete("This was used for editing in Microsoft CMS")]
        public string Text { get; set; }
        
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
        /// Loads the content from Umbraco in the same way MCMS would have loaded it from its node cache.
        /// </summary>
        private void LoadContentFromUmbraco()
        {
            if (String.IsNullOrWhiteSpace(this.PlaceholderToBind)) return;
            if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return;

            var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            if (content == null) return;

            _value = content.GetPropertyValue<string>(this.PlaceholderToBind + "_Content");
        }

        /// <summary>
        /// Adds the HTML from the field, and raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(_value))
            {
                Controls.Add(new LiteralControl(_value));
            }

            base.OnPreRender(e);
        }

    }
}
