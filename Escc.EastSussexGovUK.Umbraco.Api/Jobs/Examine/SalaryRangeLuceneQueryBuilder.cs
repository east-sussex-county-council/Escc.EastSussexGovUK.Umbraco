using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine
{
    /// <summary>
    /// Builds a query in Lucene syntax for searching by salary range
    /// </summary>
    public class SalaryRangeLuceneQueryBuilder : ISalaryRangeQueryBuilder
    {
        /// <summary>
        /// Builds a query in Lucene syntax for searching by salary range
        /// </summary>
        /// <param name="salaryRanges">The salary ranges.</param>
        /// <returns></returns>
        public string SalaryIsWithinAnyOfTheseRanges(IList<string> salaryRanges)
        {
            var rangeQueries = new List<string>();
            foreach (var salaryRange in salaryRanges)
            {
                // Try to match salary ranges
                if (MatchRangeBetweenTwoNumbers(salaryRange, rangeQueries))
                {
                    continue;
                }

                // If no match, try a range with no upper limit
                if (MatchRangeWithLowerLimitOnly(salaryRange, rangeQueries))
                {
                    continue;
                }

                // If no match, treat it as a text string
                var sanitisedRange = Regex.Replace(salaryRange, "[^A-Za-z0-9' -]", String.Empty); // sanitise input
                rangeQueries.Add($"(+salaryRange:\"{sanitisedRange}\")");
            }

            var query = String.Empty;
            if (rangeQueries.Count > 0)
            {
                query = " +(+(" + String.Join(" ", rangeQueries.ToArray()) + "))";
            }
            return query;
        }

        private bool MatchRangeWithLowerLimitOnly(string salaryRange, List<string> rangeQueries)
        {
            var numericRange = Regex.Match(salaryRange.Replace(",", String.Empty), "^£([0-9]+) and over$");
            if (numericRange.Success)
            {
                try
                {
                    // Invent an upper limit which is Lucene's maximum. This means we can use the same query as a range, which is 
                    // convenient but also essential to capture salaries which start below the desired range but cross over into it.
                    var from = Int32.Parse(numericRange.Groups[1].Value, CultureInfo.InvariantCulture);
                    rangeQueries.Add(NumericRangeInLuceneSyntax(from, 9999999));
                    return true;
                }
                catch (FormatException)
                {
                    // just ignore bad input
                    return true;
                }
                catch (OverflowException)
                {
                    // just ignore bad input
                    return true;
                }
            }

            return false;
        }

        private static bool MatchRangeBetweenTwoNumbers(string salaryRange, IList<string> rangeQueries)
        {
            var numericRange = Regex.Match(salaryRange.Replace(",", String.Empty), "^£([0-9]+) to £?([0-9]*)$");
            if (numericRange.Success)
            {
                try
                {
                    // The query MUST match ANY ONE of the following FOUR scenarios:
                    // - a salary entirely within the range
                    // - a minimum salary below the range but a maximum within it
                    // - a minimum salary within the range but a maximum above it
                    // - a minimum salary below the range and a maximum above it

                    var from = Int32.Parse(numericRange.Groups[1].Value, CultureInfo.InvariantCulture);
                    var to = String.IsNullOrEmpty(numericRange.Groups[2].Value) ? 9999999 : Int32.Parse(numericRange.Groups[2].Value, CultureInfo.InvariantCulture);
                    rangeQueries.Add(NumericRangeInLuceneSyntax(from,to));
                    return true;
                }
                catch (FormatException)
                {
                    // just ignore bad input
                    return true;
                }
                catch (OverflowException)
                {
                    // just ignore bad input
                    return true;
                }
            }
            return false;
        }

        private static string NumericRangeInLuceneSyntax(int from, int to)
        {
            var query = $"(+(+salaryMin:[{@from.ToString("D7")} TO 9999999] +salaryMin:[0000000 TO {to.ToString("D7")}]))" + // minimum within
                    $" (+(+salaryMax:[{@from.ToString("D7")} TO 9999999] +salaryMax:[0000000 TO {to.ToString("D7")}]))"; // maximum within
            if (to < 9999999)
            {
                query += $" (+(+salaryMin:[0000000 TO {@from.ToString("D7")}] +salaryMax:[{to.ToString("D7")} TO 9999999]))"; // spans the entire range
            }
            return query;
        }
    }
}