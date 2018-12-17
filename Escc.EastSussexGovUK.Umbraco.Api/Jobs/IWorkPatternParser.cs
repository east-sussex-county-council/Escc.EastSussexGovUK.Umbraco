using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
{
    /// <summary>
    /// Parse work pattern details from a text description or surrounding HTML
    /// </summary>
    public interface IWorkPatternParser
    {
        /// <summary>
        /// Parses work pattern details from their surrounding HTML.
        /// </summary>
        /// <param name="jobAdvertHtml">The raw HTML of a TalentLink job advert</param>
        /// <returns></returns>
        WorkPattern ParseWorkPatternFromHtml(string jobAdvertHtml);

        /// <summary>
        /// Parses a work pattern from a description.
        /// </summary>
        /// <param name="workPatternDescription">The description.</param>
        WorkPattern ParseWorkPatternFromDescription(string workPatternDescription);
    }
}