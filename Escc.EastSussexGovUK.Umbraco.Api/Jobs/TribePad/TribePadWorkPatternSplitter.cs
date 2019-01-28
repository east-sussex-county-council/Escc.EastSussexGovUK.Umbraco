using System;
using System.Collections.Generic;
using System.Linq;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    /// TribePad does not natively support multiple work patterns for a job, so combinations are encoded into single work patterns eg "Full time or part time"
    /// </summary>
    public class TribePadWorkPatternSplitter : IWorkPatternSplitter
    {
        /// <summary>
        /// Splits any combined work patterns in a collection into individual work patterns
        /// </summary>
        /// <param name="workPatterns"></param>
        /// <returns></returns>
        public IList<JobsLookupValue> SplitWorkPatterns(IList<JobsLookupValue> workPatterns)
        {
            var individualWorkPatterns = new List<JobsLookupValue>();
            var splitWorkPatterns = new List<JobsLookupValue>();
            foreach (var originalWorkPattern in workPatterns)
            {
                var split = originalWorkPattern.Text.Split(new[] { " OR ", " Or ", " or ", " - " }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 1)
                {
                    individualWorkPatterns.Add(originalWorkPattern);
                }
                else
                {
                    foreach (var splitResult in split)
                    {
                        splitWorkPatterns.Add(new JobsLookupValue()
                        {
                            FieldId = originalWorkPattern.FieldId,
                            LookupValueId = originalWorkPattern.LookupValueId,
                            Text = splitResult
                        });
                    }
                }
            }

            // Avoid duplicates, eg "Full time" occurring on its own and as part of "Full time or part time" and,
            // when avoiding, favour the single values which have their original id rather than the split copy.
            foreach (var splitResult in splitWorkPatterns)
            {
                if (individualWorkPatterns.Count(x => x.Text.ToUpperInvariant() == splitResult.Text.ToUpperInvariant()) == 0)
                {
                    individualWorkPatterns.Add(splitResult);
                }
            }

            return individualWorkPatterns;
        }
    }
}