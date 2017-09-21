using System;
using System.Configuration;
using System.Xml.XPath;
using System.Globalization;

namespace Escc.EastSussexGovUK.Umbraco.Views.TermDates
{
    /// <summary>
    /// Displays a quick answer box with the next start or end or term
    /// </summary>
    public partial class QuickAnswer : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets the source of the term dates XML data.
        /// </summary>
        /// <value>
        /// The term dates data provider.
        /// </value>
        public ITermDatesDataProvider TermDatesDataProvider { get; set; }

        /// <summary>
        /// Populates the quick answer box with the next start or end or term
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.quickAnswer.Visible = false;
            if (TermDatesDataProvider == null) TermDatesDataProvider = new LocalXmlFileProvider();

            var termDatesXml = TermDatesDataProvider.GetXPathData();
            var schoolYears = termDatesXml.CreateNavigator().Select("/TermDates/SchoolYear");
            while (schoolYears.MoveNext())
            {
                // Ignore any school year once we're into the following August or beyond. This means the XML file can retain the old data.
                if (IsPastSchoolYear(schoolYears.Current))
                {
                    continue;
                }

                var terms = schoolYears.Current.SelectChildren("Term", String.Empty);
                while (terms.MoveNext())
                {
                    var startDate = DateTime.Parse(terms.Current.GetAttribute("startDate", String.Empty));
                    var endDate = DateTime.Parse(terms.Current.GetAttribute("endDate", String.Empty));

                    if (!quickAnswer.Visible)
                    {
                        if (DateTime.Now.Date >= startDate && DateTime.Now.Date <= endDate)
                        {
                            quickAnswerText.Text = "School term ends on ";
                            quickAnswerDatum.InnerText = endDate.ToString("dddd d MMMM");
                            quickAnswer.Visible = true;
                        }
                        else if (DateTime.Now.Date < startDate)
                        {
                            quickAnswerText.Text = "School term starts on ";
                            quickAnswerDatum.InnerText = startDate.ToString("dddd d MMMM");
                            quickAnswer.Visible = true;
                        }
                    }
                }
            }
        }

        private static bool IsPastSchoolYear(XPathNavigator schoolYearNode)
        {
            var startYear = Int32.Parse(schoolYearNode.GetAttribute("startYear", String.Empty), CultureInfo.CurrentCulture);
            return (DateTime.Now.Year == (startYear + 1) && DateTime.Now.Month >= 8) || (DateTime.Now.Year > (startYear + 1));
        }
    }
}