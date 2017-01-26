using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Exceptionless.Extensions;
using HtmlAgilityPack;
using Humanizer;
using Microsoft.Owin.Security.Provider;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.TalentLink
{
    /// <summary>
    /// Parse work pattern details from a text description or surrounding HTML in a TalentLink job advert
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Jobs.IWorkPatternParser" />
    public class TalentLinkWorkPatternParser : IWorkPatternParser
    {
        /// <summary>
        /// Parses work pattern details from their surrounding HTML.
        /// </summary>
        /// <param name="jobAdvertHtml">The raw HTML of a TalentLink job advert</param>
        /// <returns></returns>
        public WorkPattern ParseWorkPatternFromHtml(string jobAdvertHtml)
        {
            // The info could be in a line starting "Working pattern:" or "Hours of work:". Prefer the first one because,
            // when both are present, the words "part time" or "full time" are more likely to be there. Can't just search the
            // whole body text because there are standard clauses like "if this job is part time then..."
            var matchInBodyText = Regex.Match(jobAdvertHtml, $@"Working pattern:\s*(.*?)(<br>|<br />|</p>|{Environment.NewLine})", RegexOptions.IgnoreCase);
            if (matchInBodyText.Success)
            {
                return ParseWorkPatternFromDescription(matchInBodyText.Groups[1].Value);
            }

            matchInBodyText = Regex.Match(jobAdvertHtml, $@"Hours of work:\s*(.*?)(<br>|<br />|</p>|{Environment.NewLine})", RegexOptions.IgnoreCase);
            if (matchInBodyText.Success)
            {
                return ParseWorkPatternFromDescription(matchInBodyText.Groups[1].Value);
            }
            
            return new WorkPattern();
        }

        /// <summary>
        /// Parses a work pattern from a single-line description.
        /// </summary>
        /// <param name="workPatternDescription">The description.</param>
        /// <returns></returns>
        public WorkPattern ParseWorkPatternFromDescription(string workPatternDescription)
        {
            var parseThis = HttpUtility.HtmlDecode(workPatternDescription.Trim());

            if (Regex.IsMatch(parseThis, "Full or part( |-)time", RegexOptions.IgnoreCase))
            {
                return new WorkPattern() { IsFullTime = true, IsPartTime = true };
            }
            if (Regex.IsMatch(parseThis, "Full( |-)time", RegexOptions.IgnoreCase))
            {
                return new WorkPattern() { IsFullTime = true };
            }
            if (Regex.IsMatch(parseThis, "(Casual|Part( |-)time)", RegexOptions.IgnoreCase))
            {
                return new WorkPattern() { IsPartTime = true };
            }

            var match = Regex.Match(parseThis, @"(([0-9]*)\s*?-\s*)?([0-9]+) hours per week", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                try
                {
                    var maxHours = Int32.Parse(match.Groups[3].Value, CultureInfo.CurrentCulture);
                    var minHours = String.IsNullOrEmpty(match.Groups[2].Value) ? maxHours : Int32.Parse(match.Groups[2].Value, CultureInfo.CurrentCulture);

                    var workPattern = new WorkPattern();
                    const int fullTimeThreshold = 35;
                    if (maxHours >= fullTimeThreshold) workPattern.IsFullTime = true;
                    if (minHours < fullTimeThreshold) workPattern.IsPartTime = true;
                    return workPattern;
                }
                catch (FormatException)
                {
                    // ignore this unrecognised format
                }
                catch (OverflowException)
                {
                    // ignore this unrecognised format
                }
            }
            
            return new WorkPattern();
        }
    }
}