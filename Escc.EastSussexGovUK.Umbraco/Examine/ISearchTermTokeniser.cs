using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Splits a search term into its recognised parts
    /// </summary>
    public interface ISearchTermTokeniser
    {
        /// <summary>
        /// Tokenises the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        IList<string> Tokenise(string searchTerm);
    }
}