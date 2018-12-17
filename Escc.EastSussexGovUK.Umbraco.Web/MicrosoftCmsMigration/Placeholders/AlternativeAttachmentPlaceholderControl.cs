using System;
using System.Globalization;
using System.Web.UI.HtmlControls;

namespace Escc.EastSussexGovUK.Umbraco.Web.MicrosoftCmsMigration.Placeholders
{
    public class AlternativeAttachmentPlaceholderControl : SingleAttachmentPlaceholderControl
    {
        /// <summary>
        /// Adds a link to the media item, and raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Web.UI.HtmlControls.HtmlAnchor.set_Title(System.String)")]
        protected override void OnPreRender(EventArgs e)
        {
            if (Resource != null)
            {
                // get file extension
                string format = Resource.Values["umbracoExtension"].ToLower(CultureInfo.CurrentCulture);

                // start with icon for file format
                using (HtmlGenericControl also = new HtmlGenericControl("span"))
                {
                    also.Attributes.Add("class", "downloadAlso");
                    also.InnerText = "Also in: ";
                    this.Controls.Add(also);
                }

                // link to file using name of format
                using (HtmlAnchor link = new HtmlAnchor())
                {
                    link.Title = "View "; // default action
                    switch (format)
                    {
                        case "rtf":
                            link.InnerText = "Rich text";
                            link.Attributes["type"] = "application/rtf";
                            break;
                        case "doc":
                            link.InnerText = "Word";
                            link.Attributes["type"] = "application/msword";
                            break;
                        case "xls":
                            link.InnerText = "Excel";
                            link.Attributes["type"] = "application/excel";
                            break;
                        case "pdf":
                            link.InnerText = "Acrobat (PDF)";
                            link.Attributes["type"] = "application/pdf";
                            break;
                        case "ppt":
                            link.InnerText = "PowerPoint";
                            link.Attributes["type"] = "application/powerpoint";
                            break;
                        case "mp3":
                            link.InnerText = "MP3";
                            link.Attributes["type"] = "audio/mpeg3";
                            link.Title = "Listen to "; // change action
                            break;
                        case "wma":
                            link.InnerText = "Windows Media";
                            link.Attributes["type"] = "audio/x-ms-wma";
                            link.Title = "Listen to "; // change action
                            break;
                        case "xml":
                            link.InnerText = "XML";
                            link.Attributes["type"] = "text/xml";
                            break;
                        default:
                            link.InnerText = "Alternative format";
                            break;
                    }
                    link.Attributes["class"] = format + " no-meta";
                    link.HRef = AttachmentUrl.ToString();
                    string docTitle = "'" + Resource.Name + "'";
                    link.Title += docTitle + " in " + link.InnerText + " format";
                    this.Controls.Add(link);


                    // display the file size in brackets
                    string size = CmsUtilities.GetResourceFileSize(Resource);
                    if (size.Length > 0)
                    {
                        using (var sizeElement = new HtmlGenericControl("span"))
                        {
                            ;
                            sizeElement.InnerText = " (" + size + ")";
                            sizeElement.Attributes.Add("class", "downloadSize");
                            link.Controls.Add(sizeElement);
                        }
                    }
                }
                this.Visible = true;
            }
            else this.Visible = false;
        }
    }
}