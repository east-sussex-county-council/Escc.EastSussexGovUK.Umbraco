using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration;
using Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration.Placeholders;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    /// <summary>
    /// CMS template for forms
    /// </summary>
    public partial class FormDownloadPage : ViewUserControl<MicrosoftCmsViewModel>
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // check whether to display guidance notes
            bool guidanceDocs = (this.guidance1.HasContent || this.guidance2.HasContent || this.guidance3.HasContent || this.guidance4.HasContent || this.guidance5.HasContent);
            this.guidanceNotes.Visible = guidanceDocs;
            this.guidance.Visible = (guidanceDocs || this.phGuidance.HasContent);

            // get placeholders/values for ways to complete the form
            var placeholders = CmsUtilities.Placeholders;
            string xhtmlUrl = FormUrlPlaceholderControl.GetValue(placeholders["phDefXhtmlUrl"]);
            string uploadUrl = FormUrlPlaceholderControl.GetValue(placeholders["phDefUploadUrl"]);

            int ways = 0;
            if (phPdf.AttachmentUrl != null)
            {
                ways++;
            }
            if (phRtf.AttachmentUrl != null)
            {
                ways++;
            }
            if (phRtfSign.AttachmentUrl != null)
            {
                ways++;
            }
            if (phXls.AttachmentUrl != null)
            {
                ways++;
            }
            if (phXlsPrint.AttachmentUrl != null)
            {
                ways++;
            }
            if (xhtmlUrl.Length > 0) ways++;
            if (uploadUrl.Length > 0) ways++;
            switch (ways)
            {
                case 0:
                    this.fourWays.Text = FormDownloadPageResources.FormAttachmentWays0;
                    break;               
                case 1:
                    this.fourWays.Text = FormDownloadPageResources.FormAttachmentWays1;
                    break;               
                case 2:
                    this.fourWays.Text = FormDownloadPageResources.FormAttachmentWays2;
                    break;               
                case 3:
                    this.fourWays.Text = FormDownloadPageResources.FormAttachmentWays3;
                    break;               
                case 4:
                    this.fourWays.Text = FormDownloadPageResources.FormAttachmentWays4;
                    break;               
                case 5:
                    this.fourWays.Text = FormDownloadPageResources.FormAttachmentWays5;
                    break;               
                case 6:
                    this.fourWays.Text = FormDownloadPageResources.FormAttachmentWays6;
                    break;               
                case 7:
                    this.fourWays.Text = FormDownloadPageResources.FormAttachmentWays7;
                    break;
            }

            if (ways == 0)
            {
                formatHeading.Visible = false;
            }

            this.privacy.Visible = (xhtmlUrl.Length > 0 || uploadUrl.Length > 0);
        }
    }
}
