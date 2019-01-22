using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
{
    /// <summary>
    /// Parse work pattern details from source data
    /// </summary>
    public interface IWorkPatternParser
    {
        /// <summary>
        /// Parses work pattern details from source data.
        /// </summary>
        /// <param name="sourceData">The source data of a job advert, eg XML or JSON</param>
        /// <returns></returns>
        Task<WorkPattern> ParseWorkPattern(string sourceData);
    }
}