using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Humanizer;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    ///  Parse a work pattern from the XML of a job returned by TribePad
    /// </summary>
    public class TribePadWorkPatternParser : IWorkPatternParser
    {
        private readonly IJobsLookupValuesProvider _lookupValuesProvider;
        private readonly IWorkPatternSplitter _workPatternSplitter;
        private IEnumerable<JobsLookupValue> _workPatterns;

        /// <summary>
        /// Initializes a new instance of the <see cref="TribePadWorkPatternParser"/> class.
        /// </summary>
        /// <param name="lookupValuesProvider">A method of supplying lookup values for identifiers referenced by the job data</param>
        /// <param name="workPatternSplitter">A method of splitting apart multiple work patterns encoded into one work pattern</param>
        /// <exception cref="ArgumentNullException">lookupValuesProvider or workPatternSplitter</exception>
        public TribePadWorkPatternParser(IJobsLookupValuesProvider lookupValuesProvider, IWorkPatternSplitter workPatternSplitter)
        {
            _lookupValuesProvider = lookupValuesProvider ?? throw new ArgumentNullException(nameof(lookupValuesProvider));
            _workPatternSplitter = workPatternSplitter ?? throw new ArgumentNullException(nameof(workPatternSplitter));
        }

        /// <summary>
        /// Parse a work pattern from the XML of a job returned by TribePad
        /// </summary>
        /// <param name="sourceData">XML for a single job, with a root element of &lt;job&gt;</param>
        /// <returns></returns>
        public async Task<WorkPattern> ParseWorkPattern(string sourceData)
        {
            if (_workPatterns == null)
            {
                _workPatterns = await _lookupValuesProvider.ReadWorkPatterns();
            }

            var jobXml = XDocument.Parse(sourceData);

            var workPattern = new WorkPattern();

            decimal hoursPerWeek;
            if (Decimal.TryParse(jobXml.Root.Element("shift_hours")?.Value, out hoursPerWeek))
            {
                workPattern.HoursPerWeek = hoursPerWeek;
            }

            // Get any work pattern because we want the field id, and they all have it
            var exampleWorkPattern = _workPatterns?.FirstOrDefault();
            if (exampleWorkPattern != null)
            {
                // Use that field id to look up the work pattern id in the jobs XML
                var workPatternId = jobXml.Root.Element("custom_" + exampleWorkPattern.FieldId)?.Element("answer")?.Value;
                if (!String.IsNullOrEmpty(workPatternId))
                {
                    var matchingWorkPattern = _workPatterns.SingleOrDefault(x => x.LookupValueId == workPatternId);
                    if (matchingWorkPattern != null)
                    {
                        // TribePad doesn't support multiple work patterns, so extra work patterns have been set up 
                        // which are actually a combination of two work patterns - split them here
                        var individualWorkPatterns = matchingWorkPattern.Text.Split(new[] { " or "," - " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var individualWorkPattern in individualWorkPatterns)
                        {
                            workPattern.WorkPatterns.Add(individualWorkPattern.ToLowerInvariant().Humanize(LetterCasing.Sentence));
                        }
                    }
                }
            }

            // If no work pattern was provided but we know how many hours per week, we can add a sensible work pattern
            if (!workPattern.WorkPatterns.Any() && workPattern.HoursPerWeek.HasValue)
            {
                workPattern.WorkPatterns.Add(workPattern.HoursPerWeek.Value >= 35 ? WorkPattern.FULL_TIME : WorkPattern.PART_TIME);
            }

            return workPattern;
        }
    }
}