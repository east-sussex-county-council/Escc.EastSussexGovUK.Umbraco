using System;
using System.Collections.Specialized;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    internal class FakeSearchQueryConverter : IJobSearchQueryConverter
    {
        public FakeSearchQueryConverter()
        {
        }

        public NameValueCollection ToCollection(JobSearchQuery query)
        {
            var collection = HttpUtility.ParseQueryString(String.Empty);
            collection["keywords"] = query.Keywords;
            return collection;
        }

        public JobSearchQuery ToQuery(NameValueCollection collection)
        {
            throw new NotImplementedException();
        }
    }
}