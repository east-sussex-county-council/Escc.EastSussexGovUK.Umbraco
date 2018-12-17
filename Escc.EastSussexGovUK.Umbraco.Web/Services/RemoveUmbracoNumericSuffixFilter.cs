using System;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Umbraco.Web.Services
{
    /// <summary>
    /// Remove the number at the end of the page title if it looks like the number Umbraco adds for duplicate page names.
    /// </summary>
    /// <remarks>
    /// We expect to have duplicate sibling page names when running Google Analytics experiments.
    /// Don't be too aggressive with a [0-9]+ though, as we need to support, for example, pages ending in a year like (2015).
    /// </remarks>
    public class RemoveUmbracoNumericSuffixFilter : IPageTitleFilter
    {
        /// <summary>
        /// Applies the filter to the specified page title and returns the modified result
        /// </summary>
        /// <param name="pageTitle">The page title.</param>
        /// <returns></returns>
        public string Apply(string pageTitle)
        {
            return Regex.Replace(pageTitle, @" \([0-9]\)$", String.Empty);
        }
    }
}