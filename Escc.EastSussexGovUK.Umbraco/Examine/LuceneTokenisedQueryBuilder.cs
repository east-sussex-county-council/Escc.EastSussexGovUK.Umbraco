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
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="isRequired">if set to <c>true</c> at least one search term was matched.</param>
        /// <returns></returns>
        public string AnyOfTheseTermsInThisField(IList<string> searchTerms, string fieldName, bool isRequired)
        {
            var query = String.Empty;
            if (searchTerms.Count > 0)
            {
                query = $"({fieldName}:{String.Join($" {fieldName}:", searchTerms.ToArray())})";
                query = PrependRequiredIndicator(isRequired, query);
            }
            return query;
        }

        /// <summary>
        /// All of the search terms must match in the given field.
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="isRequired">if set to <c>true</c> all search terms were matched.</param>
        public string AllOfTheseTermsInThisField(IList<string> searchTerms, string fieldName, bool isRequired)
        {
            var query = String.Empty;
            if (searchTerms.Count > 0)
            {
                query = $"(+{fieldName}:{String.Join($" +{fieldName}:", searchTerms.ToArray())})";
                query = PrependRequiredIndicator(isRequired, query);
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
        /// <param name="fieldNames">The field names.</param>
        /// <param name="isRequired">if set to <c>true</c> at least one search term was matched.</param>
        /// <returns></returns>
        public string AnyOfTheseTermsInAnyOfTheseFields(IList<string> searchTerms, IList<string> fieldNames, bool isRequired)
        {
            var query = String.Empty;
            if (searchTerms.Count == 0 || fieldNames.Count == 0) return query;

            var clauses = new List<string>();
            foreach (var fieldName in fieldNames)
            {
                clauses.Add(AnyOfTheseTermsInThisField(searchTerms, fieldName, false).Trim());
            }
            query = "(" + String.Join(" ", clauses.ToArray()) + ")";
            query = PrependRequiredIndicator(isRequired, query);
            return query;
        }

        /// <summary>
        /// All of the search terms must match in any one of the given fields. Matching one term in one field and another in a second field will not be counted.
        /// </summary>
        /// <param name="searchTerms">The search terms.</param>
        /// <param name="fieldNames">The field names.</param>
        /// <param name="isRequired">if set to <c>true</c> all search terms were matched.</param>
        /// <returns></returns>
        public string AllOfTheseTermsInAnyOfTheseFields(IList<string> searchTerms, IList<string> fieldNames, bool isRequired)
        {
            var query = String.Empty;
            if (searchTerms.Count == 0 || fieldNames.Count == 0) return query;

            var clauses = new List<string>();
            foreach (var fieldName in fieldNames)
            {
                clauses.Add(AllOfTheseTermsInThisField(searchTerms, fieldName, false).Trim());
            }
            query = "(" + String.Join(" ", clauses.ToArray()) + ")";
            query = PrependRequiredIndicator(isRequired, query);
            return query;
        }
    }
}