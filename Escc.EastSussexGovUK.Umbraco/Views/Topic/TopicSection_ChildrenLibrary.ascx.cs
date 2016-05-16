using System;
using System.Text.RegularExpressions;
using Escc.Umbraco.MicrosoftCmsMigration.Placeholders;

namespace Escc.EastSussexGovUK.Umbraco.Views.Topic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    public partial class TopicSection_ChildrenLibrary : TopicSection
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.subtitle.PlaceholderToBind = this.PlaceholderToBindSubtitle;
            this.content.PlaceholderToBind = this.PlaceholderToBindContent;

                this.content.PreRender += content_PreRender;
        }

        void content_PreRender(object sender, EventArgs e)
        {
            var placeholder = sender as RichHtmlPlaceholderControl;
            if (placeholder != null)
            {
                html.Text = Regex.Replace(placeholder.Html, " href=\"([A-Za-z0-9/]+childrenslibrary/)([A-Za-z0-9]+)/", " class=\"$2\" href=\"$1$2/");
                placeholder.Visible = false;
            }
        }
    }
}