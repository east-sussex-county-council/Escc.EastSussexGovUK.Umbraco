using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views.Topic
{
    /// <summary>
    /// Common base class for usercontrols which determine the layout of a section on a Standard Topic Page
    /// </summary>
    public abstract class TopicSection : System.Web.UI.UserControl
    {

        /// <summary>
        /// Gets or sets the placeholder to bind for the section layout.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindSection { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the 1st image.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindImage01 { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the 2nd image.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindImage02 { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the 3rd image.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindImage03 { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the 1st caption.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindCaption01 { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the 2nd caption.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindCaption02 { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the 3rd caption.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindCaption03 { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the subtitle.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindSubtitle { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the content.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindContent { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the 1st "alt as caption" checkbox.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindAltAsCaption01 { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the 2nd "alt as caption" checkbox.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindAltAsCaption02 { get; set; }

        /// <summary>
        /// Gets or sets the placeholder to bind for the 3rd "alt as caption" checkbox.
        /// </summary>
        /// <value>The placeholder to bind.</value>
        public string PlaceholderToBindAltAsCaption03 { get; set; }

        /// <summary>
        /// Restricts the width of an image container to that of the image and its border.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="placeholderName">Name of the placeholder.</param>
        /// <param name="borderWidth">Extra widht, in pixels, to add to the container to allow for a border</param>
        public static void RestrictImageContainer(HtmlControl container, string placeholderName, int borderWidth)
        {
           if (container == null) throw new ArgumentNullException("container");
            
            if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return;

            var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            if (content == null) return;

            var imageData = content.GetPropertyValue<IPublishedContent>(placeholderName + "_Content");
            if (imageData != null)
            {
                container.Style.Add("width", (imageData.GetPropertyValue<int>("umbracoWidth") + borderWidth).ToString(CultureInfo.CurrentCulture) + "px");
                container.Style.Add("max-width", "100%"); // this allows contained image to scale down with screen size
            }

        }

        /// <summary>
        /// Displays the caption.
        /// </summary>
        /// <param name="captionControl">The caption control.</param>
        /// <param name="imagePlaceholderName">Name of the image placeholder.</param>
        /// <param name="altAsCaptionPlaceholderName">Name of the alt as caption placeholder.</param>
        protected static void DisplayCaption(TextPlaceholderControl captionControl, string imagePlaceholderName, string altAsCaptionPlaceholderName)
        {
            if (captionControl == null) throw new ArgumentNullException("captionControl");

            if (UmbracoContext.Current == null || !UmbracoContext.Current.PageId.HasValue) return;

            var content = UmbracoContext.Current.ContentCache.GetById(UmbracoContext.Current.PageId.Value);
            if (content == null) return;

            var imageData = content.GetPropertyValue<IPublishedContent>(imagePlaceholderName + "_Content");
            if (imageData == null) return;

            var altAsCaption = content.GetPropertyValue<bool>(altAsCaptionPlaceholderName + "_Content");

            if (altAsCaption)
            {
                captionControl.Visible = false;
                captionControl.Parent.Controls.Add(new LiteralControl(imageData.Name));
            }
            else
            {
                captionControl.Parent.Visible = captionControl.HasContent;
            }
        }
    }
}