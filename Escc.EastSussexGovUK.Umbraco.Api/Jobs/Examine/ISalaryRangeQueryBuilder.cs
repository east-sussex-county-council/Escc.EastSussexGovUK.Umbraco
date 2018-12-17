using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.Examine
{
    /// <summary>
    /// Builds a query for searching by salary range
    /// </summary>
    public interface ISalaryRangeQueryBuilder
    {
        /// <summary>
        /// Builds a query for searching by salary range
        /// </summary>
        /// <param name="salaryRanges">The salary ranges.</param>
        /// <returns></returns>
        string SalaryIsWithinAnyOfTheseRanges(IList<string> salaryRanges);
    }
}