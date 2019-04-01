using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Splits a keyword search term based on whitespace, with phrases in quotes preserved as a single term
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Examine.ISearchTermTokeniser" />
    public class KeywordsTokeniser : ISearchTermTokeniser
    {
        /// <summary>
        /// Tokenises the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        public IList<string> Tokenise(string searchTerm)
        {
            var terms = new List<string>();

            // sanitise the search term
            searchTerm = searchTerm.Trim();
            searchTerm = Regex.Replace(searchTerm, @"[^\w\s" + "\"/-]", String.Empty);

            if (!String.IsNullOrEmpty(searchTerm))
            {
                var termsToSplit = new List<string>();
                var pos = searchTerm.IndexOf("\"", StringComparison.CurrentCulture);
                if (pos == -1)
                {
                    termsToSplit.Add(searchTerm);
                }
                else
                {
                    while (pos > -1)
                    {
                        // Save anything before the quote
                        var before = searchTerm.Substring(0, pos).Trim();
                        if (!String.IsNullOrEmpty(before))
                        {
                            termsToSplit.Add(before);
                            searchTerm = searchTerm.Remove(0, pos).Trim();
                        }

                        // Find the end quote
                        var end = searchTerm.IndexOf("\"", 1, StringComparison.CurrentCulture);
                        if (end == -1)
                        {
                            termsToSplit.Add(searchTerm.Trim() + "\"");
                            searchTerm = String.Empty;
                            break;
                        }
                        else
                        {
                            termsToSplit.Add(searchTerm.Substring(0, end+1));
                            searchTerm = searchTerm.Remove(0, end + 1).Trim();
                        }
                        pos = searchTerm.IndexOf("\"", StringComparison.CurrentCulture);
                    }
                    if (!String.IsNullOrWhiteSpace(searchTerm))
                    {
                        termsToSplit.Add(searchTerm.Trim());                        
                    }
                }

                foreach (var term in termsToSplit)
                {
                    if (term.StartsWith("\"") && term.EndsWith("\""))
                    {
                        terms.Add(term);
                    }
                    else
                    {
                        terms.AddRange(term.Split(' '));
                    }
                }

            }
            return terms;
        }
    }
}