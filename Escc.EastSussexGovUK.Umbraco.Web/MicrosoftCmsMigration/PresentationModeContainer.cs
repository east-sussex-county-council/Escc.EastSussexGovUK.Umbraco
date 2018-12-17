using System.Web.UI.WebControls;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration
{
    /// <summary>
    /// Control which conditionally shows its child contents depending on whether the page is in preview mode
    /// </summary>
    public class PresentationModeContainer : PlaceHolder
    {
        /// <summary>
        /// Gets or sets the mode to show the child content in - either published or unpublished.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public string Mode { get; set; }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            var inPreviewMode = UmbracoContext.Current.InPreviewMode;
            if (Mode.ToUpperInvariant() == "UNPUBLISHED" && !inPreviewMode)
            {
                this.Visible = false;
            }

            base.CreateChildControls();
        }
    }
}