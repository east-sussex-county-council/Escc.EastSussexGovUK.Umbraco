using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Removes words from a string, because in Examine an exact string search containing stop words will not work
    /// </summary>
    public class LuceneStopWordsRemover : ISearchFilter
    {
        // Lucene default stopwords according to http://stackoverflow.com/questions/17527741/what-is-the-default-list-of-stopwords-used-in-lucenes-stopfilter#17531638
        private static readonly string[] StopWords = {"a", "an", "and", "are", "as", "at", "be", "but", "by",
                                    "for", "if", "in", "into", "is", "it",
                                    "no", "not", "of", "on", "or", "such",
                                    "that", "the", "their", "then", "there", "these",
                                    "they", "this", "to", "was", "will", "with"};

        /// <summary>
        /// Removes the stop words from the string and consolidates any white space
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string Filter(string value)
        {
            if (String.IsNullOrEmpty(value)) return String.Empty;

            foreach (var word in StopWords)
            {
                value = Regex.Replace(value, $@"\b{word}\b", String.Empty, RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"\s+", " ", RegexOptions.IgnoreCase);
            }
            return value;
        }
    }
}