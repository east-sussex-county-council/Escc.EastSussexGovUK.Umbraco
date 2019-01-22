using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Escc.EastSussexGovUK.Umbraco.Jobs;

namespace Escc.EastSussexGovUK.Umbraco.Api.Jobs.TribePad
{
    /// <summary>
    ///  Parse a work pattern from the XML of a job returned by TribePad
    /// </summary>
    public class TribePadWorkPatternParser : IWorkPatternParser
    {
        private readonly IJobsLookupValuesProvider _lookupValuesProvider;
        private IEnumerable<JobsLookupValue> _workPatterns;

        /// <summary>
        /// Initializes a new instance of the <see cref="TribePadWorkPatternParser"/> class.
        /// </summary>
        /// <param name="lookupValuesProvider">A method of supplying lookup values for identifiers referenced by the job data</param>
        /// <exception cref="ArgumentNullException">lookupValuesProvider</exception>
        public TribePadWorkPatternParser(IJobsLookupValuesProvider lookupValuesProvider)
        {
            _lookupValuesProvider = lookupValuesProvider ?? throw new ArgumentNullException(nameof(lookupValuesProvider));
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

            var exampleWorkPattern = _workPatterns?.FirstOrDefault();
            if (exampleWorkPattern != null)
            {
                var workPatternId = jobXml.Root.Element("custom_" + exampleWorkPattern.FieldId)?.Element("answer")?.Value;
                if (!String.IsNullOrEmpty(workPatternId))
                {
                    var matchingWorkPattern = _workPatterns.SingleOrDefault(x => x.LookupValueId == workPatternId);
                    if (matchingWorkPattern != null)
                    {
                        var workPatternComparable = matchingWorkPattern.Text.ToUpperInvariant();
                        if (workPatternComparable == "FULL TIME") workPattern.WorkPatterns.Add(WorkPattern.FULL_TIME);
                        if (workPatternComparable == "PART TIME") workPattern.WorkPatterns.Add(WorkPattern.PART_TIME);
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