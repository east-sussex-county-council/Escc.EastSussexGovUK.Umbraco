using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Removes characters from a search term that might trip up Examine or Lucene
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Examine.ISearchFilter" />
    public class SearchTermSanitiser : ISearchFilter
    {
        /// <summary>
        /// Filters the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public string Filter(string query)
        {
            if (query == null) return String.Empty;
            var regex = new Regex(@"[^\w\s-]");
            return regex.Replace(query, String.Empty).Trim();
        }
    }
}