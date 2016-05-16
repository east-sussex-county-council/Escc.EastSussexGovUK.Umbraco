using System;

namespace Escc.EastSussexGovUK.Umbraco.Views.Topic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    public partial class TopicSection_ImageRow : TopicSection
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.phImage1.PlaceholderToBind = this.PlaceholderToBindImage01;
            this.phImage2.PlaceholderToBind = this.PlaceholderToBindImage02;
            this.phImage3.PlaceholderToBind = this.PlaceholderToBindImage03;
            this.caption1.PlaceholderToBind = this.PlaceholderToBindCaption01;
            this.caption2.PlaceholderToBind = this.PlaceholderToBindCaption02;
            this.caption3.PlaceholderToBind = this.PlaceholderToBindCaption03;
            this.subtitle.PlaceholderToBind = this.PlaceholderToBindSubtitle;
            this.content.PlaceholderToBind = this.PlaceholderToBindContent;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Because we set the PlaceholderToBind property late .HasContent won't work until later in the life-cycle,
            // so work with Images in PreRender
            this.image1.Visible = this.phImage1.HasContent;
            this.image2.Visible = this.phImage2.HasContent;
            this.image3.Visible = this.phImage3.HasContent;

            this.imageRow.Visible = (this.image1.Visible || this.image2.Visible || this.image3.Visible);

            DisplayCaption(this.caption1, this.PlaceholderToBindImage01, this.PlaceholderToBindAltAsCaption01);
            DisplayCaption(this.caption2, this.PlaceholderToBindImage02, this.PlaceholderToBindAltAsCaption02);
            DisplayCaption(this.caption3, this.PlaceholderToBindImage03, this.PlaceholderToBindAltAsCaption03);

            RestrictImageContainer(this.image1, this.PlaceholderToBindImage01, 0);
            RestrictImageContainer(this.image2, this.PlaceholderToBindImage02, 0);
            RestrictImageContainer(this.image3, this.PlaceholderToBindImage03, 0);

            // Set id of subtitle so that it's compatible with existing within-page links, but do it late 
            // because if you do it during Page_Load and then access the page in Preview mode from Edit mode, 
            // the content is lost somewhere between CreatePresentationChildControls and LoadContentForPresentation.
            SectionLayoutManager.AddSuffixToPlaceholderId(this.subtitle, this.PlaceholderToBindSubtitle);

            base.OnPreRender(e);
        }

    }
}