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

            return new Salary();
        }

        /// <summary>
        /// Parses a salary from a description of the salary.
        /// </summary>
        /// <param name="salaryDescription">The salary description.</param>
        /// <returns></returns>
        public Salary ParseSalaryFromDescription(string salaryDescription)
        {
            var parsedSalary = new Salary
            {
                SalaryRange = salaryDescription
            };

            // Normalise whitespace and punctuation in the salary
            var parseThis = Regex.Replace(salaryDescription, @"\s+", " ").Replace(" - ", "-").Replace(",", String.Empty);
            parseThis = Regex.Replace(parseThis, "(£[0-9]+) ([0-9]+)", "$1$2");

            // Now try to match the various formats seen in TalentLink data
            var match = Regex.Match(parseThis, "^([0-9]+)-([0-9]+) GBP Year");
            if (match.Success)
            {
                parsedSalary.MinimumSalary = Int32.Parse(match.Groups[1].Value, CultureInfo.CurrentCulture);
                parsedSalary.MaximumSalary = Int32.Parse(match.Groups[2].Value, CultureInfo.CurrentCulture);
                parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary}–£{parsedSalary.MaximumSalary} per annum";
                return parsedSalary;
            }

            match = Regex.Match(parseThis, "([0-9]+) GBP Year");
            if (match.Success)
            {
                parsedSalary.MinimumSalary = Int32.Parse(match.Groups[1].Value, CultureInfo.CurrentCulture);
                parsedSalary.MaximumSalary = parsedSalary.MinimumSalary;
                parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary} per annum";
                return parsedSalary;
            }

            match = Regex.Match(parseThis, "£([0-9]+)( to |-)£([0-9]+)");
            if (match.Success)
            {
                parsedSalary.MinimumSalary = Int32.Parse(match.Groups[1].Value, CultureInfo.CurrentCulture);
                parsedSalary.MaximumSalary = Int32.Parse(match.Groups[3].Value, CultureInfo.CurrentCulture);
                parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary}–£{parsedSalary.MaximumSalary} per annum";
                return parsedSalary;
            }

            match = Regex.Match(parseThis, "£([0-9]+) and over$");
            if (match.Success)
            {
                parsedSalary.MinimumSalary = Int32.Parse(match.Groups[1].Value, CultureInfo.CurrentCulture);
                parsedSalary.MaximumSalary = null;
                parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary}+ per annum";
                return parsedSalary;
            }

            return parsedSalary;
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