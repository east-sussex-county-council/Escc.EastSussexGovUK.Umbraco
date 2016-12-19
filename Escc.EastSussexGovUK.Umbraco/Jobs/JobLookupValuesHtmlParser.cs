using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class JobLookupValuesHtmlParser : IJobLookupValuesParser
    {
        /// <summary>
        /// Parses the job types.
        /// </summary>
        /// <param name="sourceData">The HTML.</param>
        /// <param name="fieldName">Name of the field containing the lookup values.</param>
        /// <returns></returns>
        /// <remarks>
        /// Can't use <see cref="HtmlAgilityPack" /> here because it parses all the options as one element
        /// </remarks>
        public Dictionary<int,string> ParseLookupValues(string sourceData, string fieldName)
        {
            if (String.IsNullOrEmpty(sourceData)) return null;

            var lookupValues = new Dictionary<int,string>();

            sourceData = Regex.Replace(Regex.Replace(sourceData, "\r", String.Empty), "\n", String.Empty);

            var selectList = Regex.Match(sourceData, "<select name=\"" + fieldName + "\"[^>]*>.*?</select>", RegexOptions.Multiline);
            if (selectList.Success)
            {
                AddValuesFromSelect(selectList, lookupValues);
            }

            var radiosOrCheckboxes = Regex.Matches(sourceData, "<input [^>]*name=\"" + fieldName + "\"[^>]*value=\"([0-9]+)\"[^>]*><label[^>]*>(.*?)</label>", RegexOptions.Multiline);
            foreach (Match match in radiosOrCheckboxes)
            {
                CleanAndAddValue(match.Groups[1].Value, match.Groups[2].Value, lookupValues);
            }

            return lookupValues;
        }

        private static void AddValuesFromSelect(Match selectList, Dictionary<int, string> lookupValues)
        {
            var options = Regex.Matches(selectList.Value, "<option value=\"([0-9]+)\"[^>]*>(.*?)</option>", RegexOptions.Multiline);
            foreach (Match option in options)
            {
                CleanAndAddValue(option.Groups[1].Value, option.Groups[2].Value, lookupValues);
            }
        }

        private static void CleanAndAddValue(string matchedKey, string matchedValue, Dictionary<int, string> lookupValues)
        {
            var value = HttpUtility.HtmlDecode(matchedValue);
            value = Regex.Replace(value, @" \([0-9]+\)$", String.Empty);
            lookupValues.Add(Int32.Parse(matchedKey), value);
        }
    }
}