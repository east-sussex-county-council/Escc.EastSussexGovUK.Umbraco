using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TalentLink
{
    /// <summary>
    /// Parse salary details from a description of the salary
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.ISalaryParser" />
    public class TalentLinkSalaryFromDescriptionParser : ISalaryParser
    {
        /// <summary>
        /// Parses a salary from a description of the salary, and format it according to East Sussex County Council house style.
        /// </summary>
        /// <param name="sourceData">The salary description.</param>
        /// <returns></returns>
        public Task<Salary> ParseSalary(string sourceData)
        {
            // Keep the first line, remove any additional lines
            var isMultiLine = Regex.Match(sourceData, $"(<br>|<br />|</p>|{Environment.NewLine})");
            if (isMultiLine.Success)
            {
                sourceData = sourceData.Substring(0, isMultiLine.Index);
            }

            Salary parsedSalary = null;

            // Normalise whitespace and punctuation in the salary
            var parseThis = Regex.Replace(sourceData, @"\s+", " ").Replace(" - ", "-").Replace(",", String.Empty);
            parseThis = Regex.Replace(parseThis, "(£[0-9.]+) ([0-9.]+)", "$1$2");

            // If it's an hourly rate, stop now - don't try to work out an annual salary
            if (parseThis.Contains(" per hour"))
            {
                parsedSalary = new Salary
                {
                    SalaryRange = sourceData
                };
            }

            // Now try to match the various formats seen in TalentLink data
            if (parsedSalary == null)
            {
                var match = Regex.Match(parseThis, "^([0-9.]+)-([0-9.]+) GBP Year");
                if (match.Success)
                {
                    parsedSalary = new Salary();
                    parsedSalary.MinimumSalary = ParseSalaryValue(match.Groups[1].Value);
                    parsedSalary.MaximumSalary = ParseSalaryValue(match.Groups[2].Value);
                    if (parsedSalary.MinimumSalary == parsedSalary.MaximumSalary)
                    {
                        parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary?.ToString("n0")} per annum";
                    }
                    else
                    {
                        parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary?.ToString("n0")} to £{parsedSalary.MaximumSalary?.ToString("n0")} per annum";
                    }
                }
            }

            if (parsedSalary == null)
            {
                var match = Regex.Match(parseThis, "([0-9.]+) GBP Year");
                if (match.Success)
                {
                    parsedSalary = new Salary();
                    parsedSalary.MinimumSalary = ParseSalaryValue(match.Groups[1].Value);
                    parsedSalary.MaximumSalary = parsedSalary.MinimumSalary;
                    parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary?.ToString("n0")} per annum";
                }
            }

            if (parsedSalary == null)
            {
                var match = Regex.Match(parseThis, "£([0-9.]+)( to |-)£([0-9.]+)");
                if (match.Success)
                {
                    parsedSalary = new Salary();
                    parsedSalary.MinimumSalary = ParseSalaryValue(match.Groups[1].Value);
                    parsedSalary.MaximumSalary = ParseSalaryValue(match.Groups[3].Value);
                    parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary?.ToString("n0")} to £{parsedSalary.MaximumSalary?.ToString("n0")} per annum";
                }
            }

            if (parsedSalary == null)
            {
                var match = Regex.Match(parseThis, "£([0-9.]+) and over$");
                if (match.Success)
                {
                    parsedSalary = new Salary();
                    parsedSalary.MinimumSalary = ParseSalaryValue(match.Groups[1].Value);
                    parsedSalary.MaximumSalary = null;
                    parsedSalary.SalaryRange = $"£{parsedSalary.MinimumSalary?.ToString("n0")}+ per annum";
                }
            }

            if (parsedSalary == null)
            {
                parsedSalary = new Salary()
                {
                    SalaryRange = sourceData
                };
            }

            // In case someone puts Salary: £xxx to £xxx Note: long essay about pay scales
            var notesPosition = parsedSalary.SalaryRange.ToUpperInvariant().IndexOf("NOTE:");
            if (notesPosition > 0)
            {
                parsedSalary.SalaryRange = parsedSalary.SalaryRange.Substring(0, notesPosition).Trim();
            }

            return Task.FromResult(parsedSalary);
        }

        private static int ParseSalaryValue(string salaryValue)
        {
            var salary = Decimal.Parse(salaryValue, CultureInfo.CurrentCulture);
            return (int)Math.Floor(salary);
        }
    }
}