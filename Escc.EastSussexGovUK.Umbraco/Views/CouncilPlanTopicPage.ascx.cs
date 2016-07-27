using System;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public partial class CouncilPlanTopicPage : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CouncilPlanUtility.SetContentPolicy(Response.Headers);

            // This placeholder is intended as a title, but sometimes used as a paragraph. Assume more than 6 words means they intended a paragraph.
            var title = CmsUtilities.Placeholders["phDefPriorityTitle"].Value.ToString().Split(' ');
            if (title.Length > 6) phDefPriorityTitle.ElementName = "p";

            leader.Visible = leaderPhoto.HasContent;
            chiefExec.Visible = chiefExecPhoto.HasContent;
            logo1.Visible = logo1.HasContent;
            logo2.Visible = logo2.HasContent;
            logos.Visible = (logo1.Visible || logo2.Visible);
            image1.Visible = image1.HasContent;
            image2.Visible = image2.HasContent;
            image3.Visible = image3.HasContent;
            image4.Visible = image4.HasContent;
            images.Visible = (image1.Visible || image2.Visible || image3.Visible || image4.Visible);

            // Single column if second column not used
            var singleCol = (!aims.HasContent && !content3.HasContent && !logo1.HasContent && !logo2.HasContent);
            if (singleCol)
            {
                columns.Attributes.Remove("class");
                column1.Attributes.Remove("class");
                column2.Attributes.Remove("class");
            }
        }
    }
}