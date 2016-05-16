using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Escc.Umbraco.MicrosoftCmsMigration;
using Escc.Umbraco.MicrosoftCmsMigration.Placeholders;

namespace Escc.EastSussexGovUK.Umbraco.Views
{
    public partial class StandardDownloadPage : ViewUserControl<MicrosoftCmsViewModel>
    {
        private bool column1FormatIsConsistent;
        private bool column2FormatIsConsistent;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.latest.LatestHtml = Model.Latest.ToString();

                int downloadRowCount = 0;

                // detect formats based on first file found
                string column1format = null;
                string column2format = null;

                // hide unused rows
                for (short i = 1; i <= 40; i++)
                {
                    string idNum = i.ToString(CultureInfo.InvariantCulture);
                    if (idNum.Length == 1) idNum = "0" + idNum; // add leading 0
                    HtmlTableRow row = FindControl("row" + idNum) as HtmlTableRow;
                    if (row != null)
                    {
                        var fileExtension = GetFileExtensionFromPlaceholder("phFile" + idNum, this);
                        row.Visible = (!String.IsNullOrEmpty(fileExtension));
                        if (row.Visible)
                        {
                            // detect formats based on first file found
                            if (column1format == null)
                            {
                                column1format = fileExtension;
                            }
                            else
                            {
                                this.column1FormatIsConsistent = (this.column1FormatIsConsistent && (column1format == fileExtension || String.IsNullOrEmpty(fileExtension)));
                            }

                            fileExtension = GetFileExtensionFromPlaceholder("phFile" + idNum + "a", this);
                            if (column2format == null)
                            {
                                column2format = fileExtension;
                            }
                            else
                            {
                                this.column2FormatIsConsistent = (this.column2FormatIsConsistent && (column2format == fileExtension || String.IsNullOrEmpty(fileExtension)));
                            }
                            downloadRowCount++;
                        }
                    }
                }

                // hide unused column
                    HideUnusedColumn(this, column2format);

                    if (downloadRowCount == 0)
                    {
                        downloadList.Visible = false;
                    }

                    // show formats in column headers
                    this.col1head.InnerHtml = "<span class=\"" + column1format + "\">" + Server.HtmlEncode(FileFormatName(column1format)) + "</span>";
                    this.col0.InnerHtml = "<span class=\"" + column2format + "\">" + Server.HtmlEncode(FileFormatName(column2format)) + "</span>";

            image1.Visible = (phImage01.HasContent);
            CmsUtilities.ShowCaption(phImage01.PlaceholderToBind, "phDefAltAsCaption01", caption01, alt01);
        
        }
        
        private static string GetFileExtensionFromPlaceholder(string placeholderName, Control container)
        {
            string extension = null;

            var placeholder = container.FindControl(placeholderName) as SingleAttachmentPlaceholderControl;
            if (placeholder != null)
            {
                var url = placeholder.AttachmentUrl;

                if (url != null)
                {
                    extension = System.IO.Path.GetExtension(url.ToString()).Replace(".", "").ToLower(CultureInfo.CurrentCulture);
                }
            }
            return extension;
        }
        
        private static void HideUnusedColumn(Control content, string column2Format)
        {
            if (String.IsNullOrEmpty(column2Format))
            {
                for (short i = 0; i <= 40; i++)
                {
                    string idNum = i.ToString(CultureInfo.InvariantCulture);
                    Control cell = content.FindControl("col" + idNum);
                    if (cell != null) cell.Visible = false;
                }
            }
        }

        /// <summary>
        /// Gets the name of the format.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        private static string FileFormatName(string extension)
        {
            switch (extension)
            {
                case "pdf": return "Acrobat (PDF)";
                case "doc": return "Word";
                case "xls": return "Excel";
                case "ppt": return "PowerPoint";
                case "mov": return "QuickTime";
                case "mp3": return "MP3";
                case "wma": return "Windows Media Audio";
                case "xml": return "XML";
            }
            return String.Empty;
        }
    }
}