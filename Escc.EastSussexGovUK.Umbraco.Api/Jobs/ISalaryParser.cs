using System.Threading.Tasks;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
{
    /// <summary>
    /// Parse salary details from a text description or source data
    /// </summary>
    public interface ISalaryParser
    {
        /// <summary>
        /// Parses salary details from the source data of a job advert.
        /// </summary>
        /// <param name="sourceData">The source data of a job advert, eg XML or JSON</param>
        /// <returns></returns>
        Task<Salary> ParseSalaryFromJobAdvert(string sourceData);

        /// <summary>
        /// Parses a salary from a description of the salary.
        /// </summary>
        /// <param name="salaryDescription">The salary description.</param>
        Salary ParseSalaryFromDescription(string salaryDescription);
    }
}