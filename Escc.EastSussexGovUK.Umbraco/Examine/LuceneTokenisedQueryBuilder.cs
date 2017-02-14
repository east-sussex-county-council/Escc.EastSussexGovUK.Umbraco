using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Examine
{
    /// <summary>
    /// Build a query using Lucene syntax
    /// </summary>
    /// <seealso cref="ITokenisedQueryBuilder" />
    public class LuceneTokenisedQueryBuilder : ITokenisedQueryBuilder
    {
        /// <summary>
        /// Any of the search terms in the query must match the given field
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="field">The field to search.</param>
        /// <param name="isRequired">if set to <c>true</c> at least one search term was matched.</param>
        /// <returns></returns>
        public string AnyOfTheseTermsInThisField(IList<string> searchTerms, SearchField field, bool isRequired)
        {
            return QueryTermsInThisField(searchTerms, field, isRequired, false);
        }

        /// <summary>
        /// All of the search terms must match in the given field.
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="field">The field to search.</param>
        /// <param name="isRequired">if set to <c>true</c> all search terms were matched.</param>
        public string AllOfTheseTermsInThisField(IList<string> searchTerms, SearchField field, bool isRequired)
        {
            return QueryTermsInThisField(searchTerms, field, isRequired, true);
        }

        private static string QueryTermsInThisField(IList<string> searchTerms, SearchField field, bool wholeClauseIsRequired, bool eachTermIsRequired)
        {
            var query = String.Empty;
            if (searchTerms.Count > 0)
            {
                foreach (var searchTerm in searchTerms)
                {
                    // Lucene treats hyphens like spaces, and removing the hyphens avoids allowing a term that starts with a hyphen, which Lucene cannot parse
                    var sanitisedSearchTerm = searchTerm.Replace("-", " ");
                    sanitisedSearchTerm = sanitisedSearchTerm.Trim();
                    if (String.IsNullOrEmpty(sanitisedSearchTerm)) continue;

                    query += " ";
                    if (eachTermIsRequired) query += "+";
                    query += field.FieldName + ":" + sanitisedSearchTerm;
                    if (field.Boost != 1)
                    {
                        query += "^" + field.Boost;
                    }
                }
                query = query.Trim();
                if (!String.IsNullOrEmpty(query))
                {
                    query = $"({query})";
                    query = PrependRequiredIndicator(wholeClauseIsRequired, query);
                }
            }
            return query;
        }

        private static string PrependRequiredIndicator(bool isRequired, string query)
        {
            if (isRequired)
            {
                query = "+" + query;
            }
            query = " " + query;
            return query;
        }

        /// <summary>
        /// Any of the search terms in the query must match in at least one of the given fields
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="isRequired">if set to <c>true</c> at least one search term was matched.</param>
        /// <returns></returns>
        public string AnyOfTheseTermsInAnyOfTheseFields(IList<string> searchTerms, IList<SearchField> fields, bool isRequired)
        {
            var query = String.Empty;
            if (searchTerms.Count == 0 || fields.Count == 0) return query;

            var clauses = new List<string>();
            foreach (var field in fields)
            {
                clauses.Add(AnyOfTheseTermsInThisField(searchTerms, field, false).Trim());
            }
            query = "(" + String.Join(" ", clauses.ToArray()) + ")";
            query = PrependRequiredIndicator(isRequired, query);
            return query;
        }

        /// <summary>
        /// All of the search terms must match in any one of the given fields. Matching one term in one field and another in a second field will not be counted.
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="isRequired">if set to <c>true</c> all search terms were matched.</param>
        /// <returns></returns>
        public string AllOfTheseTermsInAnyOfTheseFields(IList<string> searchTerms, IList<SearchField> fields, bool isRequired)
        {
            if (searchTerms.Count == 0 || fields.Count == 0) return String.Empty;

            var clauses = new List<string>();
            foreach (var field in fields)
            {
                clauses.Add(AllOfTheseTermsInThisField(searchTerms, field, false).Trim());
            }
            var query = String.Join(" ", clauses.ToArray()).Trim();
            if (!String.IsNullOrEmpty(query))
            {
                query = "(" + query + ")";
                query = PrependRequiredIndicator(isRequired, query);
            }
            return query;
        }
    }
}