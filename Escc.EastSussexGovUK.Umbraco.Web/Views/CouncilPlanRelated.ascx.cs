using System;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views
{
    /// <summary>
    ///		Standard placeholders for related info
    /// </summary>
    public partial class CouncilPlanRelated : System.Web.UI.UserControl
    {

        /// <summary>
        /// Gets or sets the title of the first box
        /// </summary>
        /// <value>The title.</value>
        public string BoxTitle1 { get; set; }

        /// <summary>
        /// Gets or sets the title of the second box
        /// </summary>
        /// <value>The title.</value>
        public string BoxTitle2 { get; set; }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // add default box titles
            if (String.IsNullOrEmpty(BoxTitle1)) BoxTitle1 = "Related pages";
            if (String.IsNullOrEmpty(BoxTitle2)) BoxTitle2 = "Related websites";
            this.DataBind();

            var hasPages = phRelatedPages.HasContent;
            var hasWebsites = phRelatedSites.HasContent;

            if (hasPages && hasWebsites)
            {
                this.pages.Visible = true;
                this.websites.Visible = true;
                this.pagesOnly.Visible = false;
                this.websitesOnly.Visible = false;
            }
            else if (hasPages)
            {
                this.pages.Visible = false;
                this.websites.Visible = false;
                this.pagesOnly.Visible = true;
                this.websitesOnly.Visible = false;
            }
            else if (hasWebsites)
            {
                this.pages.Visible = false;
                this.websites.Visible = false;
                this.pagesOnly.Visible = false;
                this.websitesOnly.Visible = true;
            }

            this.related.Visible = (hasPages || hasWebsites);

        }
    }
}
