using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Helper to build up a query from raw search terms
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Examine.IQueryBuilder" />
    public class QueryBuilder : IQueryBuilder
    {
        private readonly ITokenisedQueryBuilder _tokenisedQueryBuilder;
        private readonly ISearchTermTokeniser _tokeniser;
        private readonly ISearchFilter[] _searchFilters;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="tokenisedQueryBuilder">The tokenised query builder.</param>
        /// <param name="tokeniser">The tokeniser.</param>
        /// <param name="searchFilters">The search filters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// tokenisedQueryBuilder
        /// or
        /// tokeniser
        /// </exception>
        public QueryBuilder(ITokenisedQueryBuilder tokenisedQueryBuilder, ISearchTermTokeniser tokeniser, params ISearchFilter[] searchFilters)
        {
            if (tokenisedQueryBuilder == null) throw new ArgumentNullException(nameof(tokenisedQueryBuilder));
            if (tokeniser == null) throw new ArgumentNullException(nameof(tokeniser));
            _tokenisedQueryBuilder = tokenisedQueryBuilder;
            _tokeniser = tokeniser;
            _searchFilters = searchFilters;
        }

        /// <summary>
        /// Any of the search terms in the query must match the given field
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="isRequired">if set to <c>true</c> at least one search term was matched.</param>
        /// <returns></returns>
        public string AnyOfTheseTermsInThisField(string searchTerms, string fieldName, bool isRequired)
        {
            var tokenisedKeywords = FilterAndTokenise(searchTerms);
            return _tokenisedQueryBuilder.AnyOfTheseTermsInThisField(tokenisedKeywords, fieldName, isRequired);
        }

        /// <summary>
        /// All of the search terms must match in the given field.
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="isRequired">if set to <c>true</c> all search terms were matched.</param>
        /// <returns></returns>
        public string AllOfTheseTermsInThisField(string searchTerms, string fieldName, bool isRequired)
        {
            var tokenisedKeywords = FilterAndTokenise(searchTerms);
            return _tokenisedQueryBuilder.AllOfTheseTermsInThisField(tokenisedKeywords, fieldName, isRequired);
        }

        /// <summary>
        /// Any of the search terms in the query must match in at least one of the given fields
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="fieldNames">The field names.</param>
        /// <param name="isRequired">if set to <c>true</c> at least one search term was matched.</param>
        /// <returns></returns>
        public string AnyOfTheseTermsInAnyOfTheseFields(string searchTerms, IList<string> fieldNames, bool isRequired)
        {
            var tokenisedKeywords = FilterAndTokenise(searchTerms);
            return _tokenisedQueryBuilder.AnyOfTheseTermsInAnyOfTheseFields(tokenisedKeywords, fieldNames, isRequired);
        }

        /// <summary>
        /// All of the search terms must match in any one of the given fields. Matching one term in one field and another in a second field will not be counted.
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="fieldNames">The field names.</param>
        /// <param name="isRequired">if set to <c>true</c> all search terms were matched.</param>
        /// <returns></returns>
        public string AllOfTheseTermsInAnyOfTheseFields(string searchTerms, IList<string> fieldNames, bool isRequired)
        {
            var tokenisedKeywords = FilterAndTokenise(searchTerms);
            return _tokenisedQueryBuilder.AllOfTheseTermsInAnyOfTheseFields(tokenisedKeywords, fieldNames, isRequired);
        }

        private IList<string> FilterAndTokenise(string searchTerms)
        {
            if (_searchFilters != null)
            {
                foreach (var filter in _searchFilters)
                {
                    searchTerms = filter.Filter(searchTerms);
                }
            }
            return _tokeniser.Tokenise(searchTerms);
        }
    }
}