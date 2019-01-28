using System.Collections.Generic;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs
{
    /// <summary>
    /// If combinations of work patterns are encoded into a single pattern, eg "Full time or part time", it can be useful to split them apart
    /// </summary>
    public interface IWorkPatternSplitter
    {
        /// <summary>
        /// Splits any combined work patterns in a collection into individual work patterns
        /// </summary>
        /// <param name="workPatterns"></param>
        /// <returns></returns>
        IList<JobsLookupValue> SplitWorkPatterns(IList<JobsLookupValue> workPatterns);
    }
}