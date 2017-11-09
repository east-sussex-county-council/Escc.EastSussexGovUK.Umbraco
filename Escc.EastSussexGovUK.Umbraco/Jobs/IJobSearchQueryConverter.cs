using System.Collections.Specialized;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
     /// <summary>
    /// Converts a <see cref="JobSearchQuery"/> to and from a <see cref="NameValueCollection"/> format
    /// </summary>
    public interface IJobSearchQueryConverter
    {
        /// <summary>
        /// Converts a <see cref="JobSearchQuery"/> to a <see cref="NameValueCollection"/> format
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        NameValueCollection ToCollection(JobSearchQuery query);

        /// <summary>
        /// Creates a <see cref="JobSearchQuery"/> from a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="collection">The query as a collection of parameters.</param>
        /// <returns></returns>
        JobSearchQuery ToQuery(NameValueCollection collection);
    }
}