using System;
using System.Web.Mvc;
using Escc.Umbraco.MicrosoftCmsMigration;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public partial class CouncilPlanHomePage : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CouncilPlanUtility.SetContentPolicy();

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