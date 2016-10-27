using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration.UserControls
{
    public partial class SectionNavigationMvc : ViewUserControl<SectionNavigationSettings>
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.highlightContent.PlaceholderToBind = Model.HtmlPlaceholderToBind;
            this.highlightImage.PlaceholderToBind = Model.ImagePlaceholderToBind;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // Show the box if there's content
            this.highlightImage.Visible = (this.highlightImage.HasContent && Model.EsccWebsiteView == EsccWebsiteView.Desktop);
            this.Visible = this.highlightContent.HasContent || this.highlightImage.Visible;

            // If we're going to show some content and there's an image, make sure there's only one list.
            // Otherwise the second list will wrap awkwardly below the image.
            if (this.highlightContent.HasContent && this.highlightImage.HasContent)
            {
                this.highlightContent.Html = Regex.Replace(this.highlightContent.Html, "</ul>\\s*<ul class=\"second\">", String.Empty);
            }

            // This isn't meant for a banner image but people use it that way, so handle that
            if (this.highlightImage.Visible && !this.highlightContent.HasContent)
            {
                this.highlightLinks.Attributes.Remove("class");
                this.highlightLinks.Attributes.Remove("role");
            }
        }
    }
}