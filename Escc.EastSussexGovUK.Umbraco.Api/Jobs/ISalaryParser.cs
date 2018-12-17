using Escc.EastSussexGovUK.Umbraco.Jobs;
using HtmlAgilityPack;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
{
    /// <summary>
    /// Parse salary details from a text description or surrounding HTML
    /// </summary>
    public interface ISalaryParser
    {
        /// <summary>
        /// Parses salary details from a its surrounding HTML.
        /// </summary>
        /// <param name="jobAdvertHtml">The raw HTML of a TalentLink job advert</param>
        /// <returns></returns>
        Salary ParseSalaryFromHtml(HtmlDocument jobAdvertHtml);

        /// <summary>
        /// Parses a salary from a description of the salary.
        /// </summary>
        /// <param name="salaryDescription">The salary description.</param>
        Salary ParseSalaryFromDescription(string salaryDescription);
    }
}