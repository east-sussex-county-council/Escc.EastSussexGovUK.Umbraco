using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using Escc.Dates;

namespace Escc.EastSussexGovUK.Umbraco.Web.Views.TermDates
{
    public partial class TermDatesTable : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets the source of the term dates XML data.
        /// </summary>
        /// <value>
        /// The term dates data provider.
        /// </value>
        public ITermDatesDataProvider TermDatesDataProvider { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            DisplayTable();
        }

        private void DisplayTable()
        {
            if (TermDatesDataProvider == null) TermDatesDataProvider = new LocalXmlFileProvider();

            var termDatesXml = TermDatesDataProvider.GetXPathData();
            var termDatesHtml = new StringBuilder();
            var schoolYears = termDatesXml.CreateNavigator().Select("/TermDates/SchoolYear");
            while (schoolYears.MoveNext())
            {
                var startYear = Int32.Parse(schoolYears.Current.GetAttribute("startYear", String.Empty), CultureInfo.CurrentCulture);

                // Ignore any school year once we're into the following August or beyond. This means the XML file can retain the old data.
                if (IsPastSchoolYear(schoolYears.Current))
                {
                    continue;
                }

                termDatesHtml.Append("<table>");
                termDatesHtml.Append("<caption>")
                    .Append(String.Format(CultureInfo.CurrentCulture, "September {0} to August {1}", startYear.ToString(CultureInfo.CurrentCulture), (startYear + 1).ToString(CultureInfo.CurrentCulture)))
                    .Append("</caption>").Append(Environment.NewLine);
                termDatesHtml.Append("<thead><tr><th scope=\"col\">Term</th><th scope=\"col\">Start date</th><th scope=\"col\">End date</th></tr></thead><tbody>").Append(Environment.NewLine);

                var terms = schoolYears.Current.SelectChildren(XPathNodeType.Element);
                while (terms.MoveNext())
                {
                    var name = terms.Current.GetAttribute("name", String.Empty);
                    var startDate = DateTime.Parse(terms.Current.GetAttribute("startDate", String.Empty));
                    var endDate = DateTime.Parse(terms.Current.GetAttribute("endDate", String.Empty));

                    switch (terms.Current.LocalName)
                    {
                        case "Term":

                            // Add hidden text to name so that it makes sense out of context, in iCalendar format
                            name = "<span class=\"aural\">School </span>" + Server.HtmlEncode(name);

                            break;

                        case "Holiday":

                            // Add hidden text to name so that it makes sense out of context, in iCalendar format
                            name = Server.HtmlEncode(name).Replace(" holiday", " <span class=\"aural\">school</span> holiday")
                                .Replace(" break", " <span class=\"aural\">school</span> break");

                            break;

                        case "InsetDay":

                            // Add hidden text to name so that it makes sense out of context, in iCalendar format
                            name = "<span class=\"aural\">School </span>" + Server.HtmlEncode(name);

                            break;
                    }

                    // Add hidden text to name so that it makes sense out of context, in iCalendar format
                    name += " <span class=\"aural\"> (East Sussex)</span>";

                    // Build up a unique id, which must be included for the calendar to work in Outlook 2003, and must be unique.
                    // Use a format compatible with linked data, so hopefully we can support that somewhen.
                    var uid = new StringBuilder("http://www.eastsussex.gov.uk/id/");
                    switch (terms.Current.LocalName)
                    {
                        case "Term":
                            uid.Append("schoolterm/");
                            break;
                        case "Holiday":
                            uid.Append("schoolholiday/");
                            break;
                        case "InsetDay":
                            uid.Append("schoolinsetday/");
                            break;
                    }
                    uid.Append(startYear.ToString(CultureInfo.CurrentCulture)).Append("-").Append((startYear + 1).ToString(CultureInfo.CurrentCulture)).Append("/");

                    switch (terms.Current.LocalName)
                    {
                        case "Term":
                        case "Holiday":
                            uid.Append(terms.Current.GetAttribute("name", String.Empty)
                        .ToLower(CultureInfo.CurrentCulture)
                        .Replace("term ", String.Empty)
                        .Replace(" holiday", String.Empty)
                        .Replace(" break", String.Empty));
                            break;
                        case "InsetDay":
                            uid.Append(startDate.ToIso8601Date());
                            break;
                    }
                    var singleDay = (startDate == endDate);

                    // Display row as hCalendar vevent, to be parsed into iCalendar download
                    termDatesHtml.Append("<tr class=\"vevent\"><td class=\"summary\">")
                        .Append(name)
                        .Append("</td><td");
                    if (singleDay) termDatesHtml.Append(" colspan=\"2\"");
                    termDatesHtml.Append("><time class=\"dtstart\" datetime=\"").Append(startDate.ToIso8601Date()).Append("\">")
                        .Append(HttpUtility.HtmlEncode(startDate.ToBritishDateWithDay()))
                        .Append("</time>");
                    if (!singleDay) termDatesHtml.Append("</td><td>");
                    termDatesHtml.Append("<time class=\"dtend\" datetime=\"").Append(endDate.AddDays(1).ToIso8601Date()).Append("\">");
                    if (!singleDay) termDatesHtml.Append(HttpUtility.HtmlEncode(endDate.ToBritishDateWithDay()));
                    termDatesHtml.Append("</time> <span class=\"uid\">" + uid + "</span></td></tr>")
                        .Append(Environment.NewLine);
                }

                termDatesHtml.Append("</tbody></table>").Append(Environment.NewLine).Append(Environment.NewLine);
            }
            this.html.Text = termDatesHtml.ToString();
        }

        private static bool IsPastSchoolYear(XPathNavigator schoolYearNode)
        {
            var startYear = Int32.Parse(schoolYearNode.GetAttribute("startYear", String.Empty), CultureInfo.CurrentCulture);
            return (DateTime.Now.Year == (startYear + 1) && DateTime.Now.Month >= 8) || (DateTime.Now.Year > (startYear + 1));
        }
    }
}