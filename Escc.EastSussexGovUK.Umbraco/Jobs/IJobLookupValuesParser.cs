using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public interface IJobLookupValuesParser
    {
        /// <summary>
        /// Parses the job types.
        /// </summary>
        /// <param name="sourceData">The source data for the job types.</param>
        /// <param name="fieldName">Name of the field containing the lookup values.</param>
        /// <returns></returns>
        Dictionary<int,string> ParseLookupValues(string sourceData, string fieldName);
    }
}