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
    public class TalentLinkSalaryParser : ISalaryParser
    {
        /// <summary>
        /// Parses salary details from a description of the salary.
        /// </summary>
        /// <param name="jobAdvertHtml">The raw HTML of a TalentLink job advert</param>
        /// <returns></returns>
        public Task<Salary> ParseSalaryFromJobAdvert(string sourceData)
        {
            Salary salary = null;

            // Look for a salary entered into a specific field
            var salaryText = ParseValueFromElementById(sourceData, "span", "JDText-salary");
            if (!String.IsNullOrEmpty(salaryText))
            {
                salary = ParseSalaryFromDescription(salaryText);
            }

            if (salary == null)
            {
                var matchInBodyText = Regex.Match(sourceData, $@"Salary:\s+(.*?)([0-9]+)(.*?)(<br>|<br />|</p>|{Environment.NewLine})");
                if (matchInBodyText.Success)
                {
                    salary = ParseSalaryFromDescription(HttpUtility.HtmlDecode(matchInBodyText.Groups[1].Value + matchInBodyText.Groups[2].Value + matchInBodyText.Groups[3].Value).Trim());
                }
            }

            if (salary == null)
            {
                var matchInBodyText = Regex.Match(sourceData, "£[0-9,]+ ?- ?£[0-9,]+ per annum");
                if (matchInBodyText.Success)
                {
                    salary = ParseSalaryFromDescription(HttpUtility.HtmlDecode(matchInBodyText.Groups[0].Value).Trim());
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

            return Task.FromResult(salary);
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

            Salary parsedSalary = null;

            // Normalise whitespace and punctuation in the salary
            var parseThis = Regex.Replace(salaryDescription, @"\s+", " ").Replace(" - ", "-").Replace(",", String.Empty);
            parseThis = Regex.Replace(parseThis, "(£[0-9.]+) ([0-9.]+)", "$1$2");

            // If it's an hourly rate, stop now - don't try to work out an annual salary
            if (parseThis.Contains(" per hour"))
            {
                parsedSalary = new Salary
                {
                    SalaryRange = salaryDescription
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
                    SalaryRange = salaryDescription
                };
            }

            // In case someone puts Salary: £xxx to £xxx Note: long essay about pay scales
            var notesPosition = parsedSalary.SalaryRange.ToUpperInvariant().IndexOf("NOTE:");
            if (notesPosition > 0)
            {
                parsedSalary.SalaryRange = parsedSalary.SalaryRange.Substring(0, notesPosition).Trim();
            }

            return parsedSalary;
        }

        private static int ParseSalaryValue(string salaryValue)
        {
            var salary = Decimal.Parse(salaryValue, CultureInfo.CurrentCulture);
            return (int)Math.Floor(salary);
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