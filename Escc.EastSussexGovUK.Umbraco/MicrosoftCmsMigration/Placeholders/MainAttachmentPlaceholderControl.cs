using System;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Escc.EastSussexGovUK.Umbraco.MicrosoftCmsMigration.Placeholders
{
    public class MainAttachmentPlaceholderControl : SingleAttachmentPlaceholderControl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Web.UI.HtmlControls.HtmlAnchor.set_Title(System.String)")]
        protected override void OnPreRender(EventArgs e)
        {
            if (Resource != null)
            {
                // build a definition list just for this item
                using (HtmlGenericControl dl = new HtmlGenericControl("dl"))
                {
                    using (HtmlGenericControl dt = new HtmlGenericControl("dt"))
                    {
                        using (HtmlGenericControl dd = new HtmlGenericControl("dd"))
                        {
                            dl.Controls.Add(dt);
                            dl.Controls.Add(dd);

                            // get file extension
                            string format = Resource.Values["umbracoExtension"].Replace(".", "").ToLower(CultureInfo.CurrentCulture);
                            dt.Attributes["class"] = format;

                            // link to file using name of format
                            using (HtmlAnchor link = new HtmlAnchor())
                            {
                                switch (format)
                                {
                                    case "rtf":
                                        format = "Rich text";
                                        link.Attributes["type"] = "application/rtf";
                                        break;
                                    case "doc":
                                        format = "Word";
                                        link.Attributes["type"] = "application/msword";
                                        break;
                                    case "xls":
                                        format = "Excel";
                                        link.Attributes["type"] = "application/excel";
                                        break;
                                    case "pdf":
                                        format = "Acrobat (PDF)";
                                        link.Attributes["type"] = "application/pdf";
                                        break;
                                    case "ppt":
                                        format = "PowerPoint";
                                        link.Attributes["type"] = "application/powerpoint";
                                        break;
                                    case "xml":
                                        format = "XML";
                                        link.Attributes["type"] = "text/xml";
                                        break;
                                    default:
                                        format = "Alternative format";
                                        break;
                                }
                                link.HRef = AttachmentUrl.ToString();
                                link.InnerText = Resource.Name;
                                link.Title = "View '" + link.InnerText + "' in " + format + " format";
                                dt.Controls.Add(link);
                            }

                            // display the description, if present
                            if (Resource.Values.ContainsKey("Description") && !String.IsNullOrEmpty(Resource.Values["Description"]))
                            {
                                dd.InnerHtml = HttpUtility.HtmlEncode(Resource.Values["Description"]);
                                dd.Visible = true;
                            }
                            else dd.Visible = false;
                        }
                    }
                    this.Controls.Add(dl);
                }
                this.Visible = true;
            }
            else this.Visible = false;

        }
    }
}