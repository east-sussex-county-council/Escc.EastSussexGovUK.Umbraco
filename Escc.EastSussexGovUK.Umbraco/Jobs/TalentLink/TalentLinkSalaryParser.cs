using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Exceptionless.Extensions;
using HtmlAgilityPack;
using Humanizer;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// Parse salary details from a description of the salary
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.ISalaryParser" />
    public class TalentLinkSalaryParser : ISalaryParser
    {
        /// <summary>
        /// Parses salary details from a description of the salary.
        /// </summary>
        /// <param name="jobAdvertHtml">The raw HTML of a TalentLink job advert</param>
        /// <returns></returns>
        public Salary ParseSalaryFromHtml(HtmlDocument jobAdvertHtml)
        {
            // Look for a salary entered into a specific field
            var salaryText = ParseValueFromElementById(jobAdvertHtml, "span", "JDText-salary");
            if (!String.IsNullOrEmpty(salaryText))
            {
                return ParseSalaryFromDescription(salaryText);
            }

            var matchInBodyText = Regex.Match(jobAdvertHtml.DocumentNode.OuterHtml, $"Salary: (.*?)([0-9]+)(.*?)(<br>|<br />|</p>|{Environment.NewLine})");
            if (matchInBodyText.Success)
            {
                return ParseSalaryFromDescription(HttpUtility.HtmlDecode(matchInBodyText.Groups[1].Value + matchInBodyText.Groups[2].Value + matchInBodyText.Groups[3].Value).Trim());
            }

            matchInBodyText = Regex.Match(jobAdvertHtml.DocumentNode.OuterHtml, "£[0-9,]+ ?- ?£[0-9,]+ per annum");
            if (matchInBodyText.Success)
            {
                return ParseSalaryFromDescription(HttpUtility.HtmlDecode(matchInBodyText.Groups[0].Value).Trim());
            }

            // There are no numbers to parse, so just take the first line of text
            matchInBodyText = Regex.Match(jobAdvertHtml.DocumentNode.OuterHtml, $"Salary: (.+?)(<br>|<br />|</p>|{Environment.NewLine})");
            if (matchInBodyText.Success)
            {
                return new Salary()
                {
                    SalaryRange = matchInBodyText.Groups[1].Value
                };
            }

            return new Salary();
        }

        /// <summary>
        /// Parses a salary from a description of the salary, and format it according to East Sussex County Council house style.
        /// </summary>
        /// <param name="salaryDescription">The salary description.</param>
        /// <returns></returns>
        public Salary ParseSalaryFromDescription(string salaryDescription)
        {
            // Keep the first line, remove any additional lines
            var isMultiLine = Regex.Match(salaryDescription, $"(<br>|<br />|</p>|{Environment.NewLine})");
            if (isMultiLine.Success)
            {
                salaryDescription = salaryDescription.Substring(0, isMultiLine.Index);
            }

            var parsedSalary = new Salary
            {
                SalaryRange = salaryDescription
            };

            // Normalise whitespace and punctuation in the salary
            var parseThis = Regex.Replace(salaryDescription, @"\s+", " ").Replace(" - ", "-").Replace(",", String.Empty);
            parseThis = Regex.Replace(parseThis, "(£[0-9.]+) ([0-9.]+)", "$1$2");

            // If it's an hourly rate, stop now - don't try to work out an annual salary
            if (parseThis.Contains(" per hour"))
            {
                return parsedSalary;
            }

            // Now try to match the various formats seen in TalentLink data
            var match = Regex.Match(parseThis, "^([0-9.]+)-([0-9.]+) GBP Year");
            if (match.Success)
            {
                parsedSalary.MinimumSalary = ParseSalaryValue(match.Groups[1].Value);
                parsedSalary.MaximumSalary = ParseSalaryValue(match.Groups[2].Value);
                parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary?.ToString("n0")} to £{parsedSalary.MaximumSalary?.ToString("n0")} per annum";
                return parsedSalary;
            }

            match = Regex.Match(parseThis, "([0-9.]+) GBP Year");
            if (match.Success)
            {
                parsedSalary.MinimumSalary = ParseSalaryValue(match.Groups[1].Value);
                parsedSalary.MaximumSalary = parsedSalary.MinimumSalary;
                parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary?.ToString("n0")} per annum";
                return parsedSalary;
            }

            match = Regex.Match(parseThis, "£([0-9.]+)( to |-)£([0-9.]+)");
            if (match.Success)
            {
                parsedSalary.MinimumSalary = ParseSalaryValue(match.Groups[1].Value);
                parsedSalary.MaximumSalary = ParseSalaryValue(match.Groups[3].Value);
                parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary?.ToString("n0")} to £{parsedSalary.MaximumSalary?.ToString("n0")} per annum";
                return parsedSalary;
            }

            match = Regex.Match(parseThis, "£([0-9.]+) and over$");
            if (match.Success)
            {
                parsedSalary.MinimumSalary = ParseSalaryValue(match.Groups[1].Value);
                parsedSalary.MaximumSalary = null;
                parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary?.ToString("n0")}+ per annum";
                return parsedSalary;
            }

            return parsedSalary;
        }

        private static int ParseSalaryValue(string salaryValue)
        {
            var salary = Decimal.Parse(salaryValue, CultureInfo.CurrentCulture);
            return (int)Math.Floor(salary);
        }

        private static string ParseValueFromElementById(HtmlDocument htmlDocument, string elementName, string elementId)
        {
            var node = htmlDocument.DocumentNode.SelectSingleNode($"//{elementName}[@id='{elementId}']");
            if (node != null)
            {
                return HttpUtility.HtmlDecode(node.InnerHtml.Trim().Replace("&nbsp;", " "));
            }
            return String.Empty;
        }
    }
}