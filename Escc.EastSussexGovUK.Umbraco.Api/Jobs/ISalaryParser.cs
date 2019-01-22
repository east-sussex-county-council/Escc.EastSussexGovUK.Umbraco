using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
{
    /// <summary>
    /// Parse salary details from source data
    /// </summary>
    public interface ISalaryParser
    {
        /// <summary>
        /// Parses salary details the source data.
        /// </summary>
        /// <param name="sourceData">The source data</param>
        /// <returns></returns>
        Task<Salary> ParseSalary(string sourceData);
    }
}