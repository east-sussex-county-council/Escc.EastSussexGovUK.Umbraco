using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Exceptionless.Extensions;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink
{
    /// <summary>
    /// Parse salary details from a description of the salary
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.ISalaryParser" />
    public class TalentLinkSalaryFromHtmlParser : ISalaryParser
    {
        private readonly ISalaryParser _salaryFromDescriptionParser;

        /// <summary>
        /// Creates a new instance of <see cref="TalentLinkSalaryFromHtmlParser"/>
        /// </summary>
        /// <param name="salaryFromDescriptionParser">Another salary parser which can parse a plain text description of a salary</param>
        public TalentLinkSalaryFromHtmlParser(ISalaryParser salaryFromDescriptionParser)
        {
            _salaryFromDescriptionParser = salaryFromDescriptionParser ?? throw new ArgumentNullException(nameof(salaryFromDescriptionParser));
        }

        /// <summary>
        /// Parses salary details from a description of the salary.
        /// </summary>
        /// <param name="jobAdvertHtml">The raw HTML of a TalentLink job advert</param>
        /// <returns></returns>
        public async Task<Salary> ParseSalary(string sourceData)
        {
            Salary salary = null;

            // Look for a salary entered into a specific field
            var salaryText = ParseValueFromElementById(sourceData, "span", "JDText-salary");
            if (!String.IsNullOrEmpty(salaryText))
            {
                salary = await _salaryFromDescriptionParser.ParseSalary(salaryText);
            }

            if (salary == null)
            {
                var matchInBodyText = Regex.Match(sourceData, $@"Salary:\s+(.*?)([0-9]+)(.*?)(<br>|<br />|</p>|{Environment.NewLine})");
                if (matchInBodyText.Success)
                {
                    salary = await _salaryFromDescriptionParser.ParseSalary(HttpUtility.HtmlDecode(matchInBodyText.Groups[1].Value + matchInBodyText.Groups[2].Value + matchInBodyText.Groups[3].Value).Trim());
                }
            }

            if (salary == null)
            {
                var matchInBodyText = Regex.Match(sourceData, "£[0-9,]+ ?- ?£[0-9,]+ per annum");
                if (matchInBodyText.Success)
                {
                    salary = await _salaryFromDescriptionParser.ParseSalary(HttpUtility.HtmlDecode(matchInBodyText.Groups[0].Value).Trim());
                }
            }

            // There are no numbers to parse, so just take the first line of text
            if (salary == null)
            {
                var matchInBodyText = Regex.Match(sourceData, $@"Salary:\s+(.+?)(<br>|<br />|</p>|{Environment.NewLine})");
                if (matchInBodyText.Success)
                {
                    salary = new Salary()
                    {
                        SalaryRange = matchInBodyText.Groups[1].Value
                    };
                }
            }

            // If still no salary found, is it clearly a volunteer role?
            if (salary == null)
            {
                var jobTitle = ParseValueFromElementById(sourceData, "h3", "JDText-Title");
                if (jobTitle.ToUpperInvariant().Contains("VOLUNTEER"))
                {
                    salary = new Salary()
                    {
                        MinimumSalary = 0,
                        MaximumSalary = 0,
                        SalaryRange = "Voluntary"
                    };
                }
            }

            if (salary == null)
            {
                salary = new Salary();
            }

            // Is there an hourly rate, which might be in addition to a salary?
            var matchHourlyRateInBodyText = Regex.Match(HttpUtility.HtmlDecode(sourceData), $@"hourly rate is £([0-9]+\.[0-9][0-9])");
            if (matchHourlyRateInBodyText.Success)
            {
                salary.HourlyRate = Decimal.Parse(matchHourlyRateInBodyText.Groups[1].Value);
            }

            return salary;
        }

        private static string ParseValueFromElementById(string sourceData, string elementName, string elementId)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.OptionFixNestedTags = true;
            htmlDocument.LoadHtml(sourceData);

            var node = htmlDocument.DocumentNode.SelectSingleNode($"//{elementName}[@id='{elementId}']");
            if (node != null)
            {
                return HttpUtility.HtmlDecode(node.InnerHtml.Trim().Replace("&nbsp;", " "));
            }
            return String.Empty;
        }
    }
}