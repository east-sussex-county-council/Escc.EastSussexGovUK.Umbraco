using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
{
    public interface IJobLookupValuesParser
    {
        /// <summary>
        /// Parses the job types.
        /// </summary>
        /// <param name="sourceData">The source data for the job types.</param>
        /// <param name="fieldName">Name of the field containing the lookup values.</param>
        /// <returns></returns>
        IList<JobsLookupValue> ParseLookupValues(string sourceData, string fieldName);
    }
}