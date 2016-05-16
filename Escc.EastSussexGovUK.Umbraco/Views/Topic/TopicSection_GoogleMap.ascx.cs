using System;

namespace Escc.EastSussexGovUK.Umbraco.Views.Topic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    public partial class TopicSection_GoogleMap : TopicSection
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.subtitle.PlaceholderToBind = this.PlaceholderToBindSubtitle;
            this.content.PlaceholderToBind = this.PlaceholderToBindContent;
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Set id of subtitle so that it's compatible with existing within-page links, but do it late 
            // because if you do it during Page_Load and then access the page in Preview mode from Edit mode, 
            // the content is lost somewhere between CreatePresentationChildControls and LoadContentForPresentation.
            SectionLayoutManager.AddSuffixToPlaceholderId(this.subtitle, this.PlaceholderToBindSubtitle);

            this.content.Html = this.content.Html.Replace("<a href=\"https://maps.google.co.uk", "<a class=\"embed\" href=\"https://maps.google.co.uk");

            base.OnPreRender(e);
        }
    }
}