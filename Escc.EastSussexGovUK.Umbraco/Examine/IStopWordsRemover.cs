using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Removes words from a string, because in Examine an exact string search containing stop words will not work
    /// </summary>
    public interface IStopWordsRemover
    {
        /// <summary>
        /// Removes the stop words from the string and consolidates any white space
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        string RemoveStopWords(string value);
    }
}