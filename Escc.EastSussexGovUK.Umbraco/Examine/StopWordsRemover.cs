using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Removes words from a string, because in Examine an exact string search containing stop words will not work
    /// </summary>
    public class StopWordsRemover : IStopWordsRemover
    {
        /// <summary>
        /// Removes the stop words from the string and consolidates any white space
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="stopWords">The stop words.</param>
        /// <returns></returns>
        public string RemoveStopWords(string value, IEnumerable<string> stopWords)
        {
            if (String.IsNullOrEmpty(value)) return String.Empty;

            foreach (var word in stopWords)
            {
                value = Regex.Replace(value, $@"\b{word}\b", String.Empty, RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"\s+", " ", RegexOptions.IgnoreCase);
            }
            return value;
        }
    }
}