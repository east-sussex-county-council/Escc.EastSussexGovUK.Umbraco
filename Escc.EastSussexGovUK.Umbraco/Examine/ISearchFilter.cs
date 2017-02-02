using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Modifies a string before it is used as an Examine search term
    /// </summary>
    public interface ISearchFilter
    {
        /// <summary>
        /// Applies the filter and returns the modified search term
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        string Filter(string value);
    }
}