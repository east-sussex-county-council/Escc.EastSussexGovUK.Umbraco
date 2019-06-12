using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
{
    /// <summary>
    /// Parses locations from data for a job
    /// </summary>
    public interface ILocationParser
    {
        /// <summary>
        /// Parses locations from data for a job
        /// </summary>
        /// <param name="sourceData"></param>
        IList<string> ParseLocations(string sourceData);
    }
}