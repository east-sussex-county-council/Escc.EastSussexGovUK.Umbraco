using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using AST.AzureBlobStorage.Helper;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration.Placeholders
{
    /// <summary>
    /// Substitute for Microsoft Content Management Server HTML placeholder control to enable migration of MCMS templates with minimal changes
    /// </summary>
    public class RichHtmlPlaceholderControl : PlaceHolder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RichHtmlPlaceholderControl"/> class.
        /// </summary>
        public RichHtmlPlaceholderControl()
        {
            Paragraphs = true;
        }

        #region Obsolete properties which are now part of the property editor

        /// <summary>
        /// Obsolete. Retained for backward compatibility.
        /// </summary>
        /// <value>
        /// The width of the edit control.
        /// </value>
        public int EditControlWidth { get; set; }

        /// <summary>
        /// Obsolete. Retained for backward compatibility.
        /// </summary>
        /// <value>
        /// The height of the edit control.
        /// </value>
        public int EditControlHeight { get; set; }


        /// <summary>
        /// Obsolete. Retained for backward compatibility.
        /// </summary>
        /// <value>
        /// The edit control class.
        /// </value>
        public string EditControlClass { get; set; }

        /// <summary>
        /// Obsolete. Retained for backward compatibility.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [embed video]; otherwise, <c>false</c>.
        /// </value>
        public bool EmbedVideo { get; set; }

        /// <summary>
        /// Obsolete. Retained for backward compatibility.
        /// </summary>
        /// <value>
        /// <c>true</c> if [uppercase first letter]; otherwise, <c>false</c>.
        /// </value>
        public bool UppercaseFirstLetter { get; set; }

        [Obsolete("Previously used in edit view, which is now handled by a separate property editor.")]
        public string ToolTip { get; set; }

        #endregion

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
        /// Gets or sets the HTML content of the placeholder.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        public string Html { get; set; }

        /// <summary>
        /// Gets a value indicating whether this placeholder has content.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has content; otherwise, <c>false</c>.
        /// </value>
        public bool HasContent { get; private set; }

        /// <summary>
        /// Gets or sets whether paragraphs and line breaks are allowed in the content.
        /// </summary>
        /// <value>
        ///   <c>true</c> if paragraphs allowed; otherwise, <c>false</c>.
        /// </value>
        public bool Paragraphs { get; set; }

        /// <summary>
        /// Gets or sets the HTML element to use when rendering a container element
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// Obsolete. <c>true</c> is implied by setting <see cref="ElementName"/> to a string which is not <c>null</c> or empty.
        /// </summary>
        [Obsolete("The value of this property is implied by whether ElementName is set to null or an empty string")]
        public bool RenderContainerElement { get; set; }

        /// <summary>
        /// Gets or sets the CSS classes to apply, if <see cref="ElementName"/> is set.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        public string CssClass { get; set; }

        /// <summary>
        /// Loads the content from Umbraco in the same way MCMS would have loaded it from its node cache.
        /// </summary>
        private void LoadContentFromUmbraco()
        {
            if (String.IsNullOrWhiteSpace(this.PlaceholderToBind)) return;
            if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return;

            var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            if (content == null) return;

            Html = ContentHelper.ParseContent(content.GetPropertyValue<string>(this.PlaceholderToBind + "_Content"));

            HasContent = !String.IsNullOrWhiteSpace(Html);
        }

        /// <summary>
        /// Adds the HTML from the field, and raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override void OnPreRender(EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Html))
            {
                if (!Paragraphs)
                {
                    Html = Regex.Replace(Html, "(<p>|<p [^>]*>|</p>|<br />)", String.Empty);
                }

                var htmlToRender = new StringBuilder();

                if (!String.IsNullOrEmpty(this.ElementName))
                {
                    // GS Fix: The Umbraco RTE puts P tags around content automatically, so adding a P tag here will produce an empty tag instead of wrapping the content.
                    // Therefore, if ElementName is "p", remove the surrounding P tag from the content first.
                    if (ElementName.ToLowerInvariant() == "p")
                    {
                        Html = umbraco.library.RemoveFirstParagraphTag(Html);
                    }

                    htmlToRender.Append("<").Append(ElementName.ToLowerInvariant());

                    // GS Fix: Should be testing for "Not" empty
                    if (!String.IsNullOrEmpty(ID)) htmlToRender.Append(" id=\"").Append(ID).Append("\"");
                    if (!String.IsNullOrEmpty(CssClass)) htmlToRender.Append(" class=\"").Append(CssClass).Append("\"");
                    htmlToRender.Append(">");
                }

                htmlToRender.Append(Html);

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
