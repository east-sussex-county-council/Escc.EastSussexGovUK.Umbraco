using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public class JobTypesHtmlParser : IJobTypesParser
    {
        /// <summary>
        /// Parses the job types.
        /// </summary>
        /// <param name="sourceData">The HTML.</param>
        /// <remarks>Can't use <see cref="HtmlAgilityPack"/> here because it parses all the options as one element</remarks>
        public Dictionary<int,string> ParseJobTypes(string sourceData)
        {
            if (String.IsNullOrEmpty(sourceData)) return null;

            var jobTypes = new Dictionary<int,string>();

            sourceData = Regex.Replace(Regex.Replace(sourceData, "\r", String.Empty), "\n", String.Empty);

            var select = Regex.Match(sourceData, "<select name=\"LOV40\"[^>]*>.*?</select>", RegexOptions.Multiline);
            if (!select.Success) return jobTypes;

            var options = Regex.Matches(select.Value, "<option value=\"([0-9]+)\"[^>]*>(.*?)</option>", RegexOptions.Multiline);
            foreach (Match option in options)
            {
                var jobType = HttpUtility.HtmlDecode(option.Groups[2].Value);
                jobType = Regex.Replace(jobType, @" \([0-9]+\)$", String.Empty);
                jobTypes.Add(Int32.Parse(option.Groups[1].Value), jobType);
            }

            return jobTypes;
        }
    }
}