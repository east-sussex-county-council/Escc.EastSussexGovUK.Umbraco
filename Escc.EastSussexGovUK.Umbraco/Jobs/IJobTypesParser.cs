using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Jobs
{
    public interface IJobTypesParser
    {
        /// <summary>
        /// Parses the job types.
        /// </summary>
        /// <param name="sourceData">The source data for the job types.</param>
        Dictionary<int,string> ParseJobTypes(string sourceData);
    }
}