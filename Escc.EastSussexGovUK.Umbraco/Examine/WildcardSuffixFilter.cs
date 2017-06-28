using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Adds a wildcard after each word in a search string so that, for example, "teacher" also matches "teachers"
    /// </summary>
    /// <seealso cref="ISearchFilter" />
    public class WildcardSuffixFilter : ISearchFilter
    {
        /// <summary>
        /// Applies the filter and returns the modified query
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string Filter(string value)
        {
            value = Regex.Replace(value, @"([^\s-(])\b", "$1*");
            value = value.Replace("*-", "-");
            return value;
        }
    }
}