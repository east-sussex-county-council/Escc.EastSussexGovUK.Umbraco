using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Examine;

namespace Escc.EastSussexGovUK.Umbraco.Jobs.Examine
{
    public class JobsLookupValuesFromExamine : IJobsLookupValuesProvider
    {
        private readonly ISearcher _searcher;

        public JobsLookupValuesFromExamine(ISearcher searcher)
        {
            if (searcher == null) throw new ArgumentNullException(nameof(searcher));
            _searcher = searcher;
        }

        public Task<IList<JobsLookupValue>> ReadLocations()
        {
            return Task.FromResult(new List<JobsLookupValue>() as IList<JobsLookupValue>);
        }

        public Task<IList<JobsLookupValue>> ReadJobTypes()
        {
            return Task.FromResult(new List<JobsLookupValue>() as IList<JobsLookupValue>);
        }

        public Task<IList<JobsLookupValue>> ReadOrganisations()
        {
            return Task.FromResult(new List<JobsLookupValue>() as IList<JobsLookupValue>);
        }

        public Task<IList<JobsLookupValue>> ReadSalaryRanges()
        {
            var lookups = new List<JobsLookupValue>() as IList<JobsLookupValue>;
            var examineQuery = _searcher.CreateSearchCriteria().Field("group", "SalaryRange").Compile();
            examineQuery.OrderBy("text");
            var results = _searcher.Search(examineQuery);
            foreach (var result in results)
            {
                var lookup = new JobsLookupValue()
                {
                    Text = result.Fields.ContainsKey("text") ? result["text"] : String.Empty,
                    Count = result.Fields.ContainsKey("count") ? Int32.Parse(result["count"], CultureInfo.InvariantCulture) : 0
                };
                lookups.Add(lookup);
            }
            return Task.FromResult(lookups);
        }

        public Task<IList<JobsLookupValue>> ReadWorkPatterns()
        {
            return Task.FromResult(new List<JobsLookupValue>() as IList<JobsLookupValue>);
        }
    }
}